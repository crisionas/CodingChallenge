using CC.Common;
using CC.UploadService.Interfaces;
using CC.UploadService.Models.Requests;
using CC.UploadService.Repository.Entities;
using Hangfire;

namespace CC.UploadService.DataMiners
{
    public abstract class DataMiner : IDataMiner
    {
        protected readonly ILogger Logger;
        private readonly IFileRepository _repository;
        private readonly IUserRequestSettings _userRequest;
        private readonly IBackgroundJobClient _jobClient;

        protected FileUploadRequest Request = null!;

        protected DataMiner(ILogger logger, IFileRepository repository, IUserRequestSettings userRequest, IBackgroundJobClient jobClient)
        {
            _repository = repository;
            _userRequest = userRequest;
            Logger = logger;
            _jobClient = jobClient;
        }

        #region public methods

        protected string? ParsedString;

        /// <summary>
        /// Execute the processor
        /// </summary>
        /// <param name="request">FileUploadRequest</param>
        /// <returns>Task</returns>
        public async Task<string?> ExecuteAsync(FileUploadRequest request)
        {
            string? trackId = default;
            Request = request;
            try
            {
                Logger.LogDebug("ExtractData process has started.");
                var byteArray = await ExtractData();

                Logger.LogDebug("ParseData process has started.");
                trackId = _jobClient.Enqueue(() => ParseData(byteArray));

                var fileData = AttachFileData(trackId);

                Logger.LogDebug("AnalyzeData process has started.");
                BackgroundJob.ContinueJobWith(trackId, () => AnalyzeData());

                Logger.LogDebug("SaveData process has started.");
                BackgroundJob.ContinueJobWith(trackId, () => SaveData(fileData));
            }
            catch (Exception e)
            {
                Logger.LogError(e, "ExecuteAsync error");
            }

            return trackId;
        }

        /// <summary>
        /// Extract the data from FileUploadRequest
        /// </summary>
        /// <returns>Task</returns>
        public async Task<byte[]> ExtractData()
        {
            var stream = new MemoryStream();
            await Request.File?.CopyToAsync(stream)!;
            return stream.ToArray();
        }

        /// <summary>
        /// Parse data from MemoryStream
        /// </summary>
        /// <returns>void</returns>
        public abstract void ParseData(byte[] byteArray);

        /// <summary>
        /// Analyze the data, in case you need some validations within the system
        /// </summary>
        /// <returns>Task</returns>
        public abstract void AnalyzeData();

        /// <summary>
        /// Save the data
        /// </summary>
        /// <returns>Task</returns>
        public async Task SaveData(FileData file)
        {
            file.Data = ParsedString;
            await _repository.SaveOrUpdateAsync(file);
            Logger.LogDebug("SaveData process finished successfully.");
        }

        #endregion

        #region private methods, that needs to be hidden and is the same for all

        /// <summary>
        /// Attach file data
        /// </summary>
        /// <param name="jobId">Job id</param>
        /// <returns>FileData</returns>
        private FileData AttachFileData(string jobId)
        {
            return new FileData
            {
                Username = _userRequest.Username,
                FileId = jobId,
                Name = Request.Name,
                Description = Request.Description,
                LastUpdate = DateTime.UtcNow
            };
        }
        #endregion
    }
}
