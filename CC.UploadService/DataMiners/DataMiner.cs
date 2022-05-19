using CC.Common;
using CC.UploadService.Interfaces;
using CC.UploadService.Models.Requests;
using CC.UploadService.Repository.Entities;

namespace CC.UploadService.DataMiners
{
    public abstract class DataMiner : IDataMiner
    {
        protected readonly ILogger Logger;
        private readonly IFileRepository _repository;
        private readonly IUserRequestSettings _userRequest;

        protected FileUploadRequest Request = null!;

        protected DataMiner(ILogger logger, IFileRepository repository, IUserRequestSettings userRequest)
        {
            _repository = repository;
            _userRequest = userRequest;
            Logger = logger;
        }

        #region public methods
        /// <summary>
        /// Execute the processor
        /// </summary>
        /// <param name="request">FileUploadRequest</param>
        /// <returns>Task</returns>
        public async Task ExecuteAsync(FileUploadRequest request)
        {
            Request = request;

            try
            {
                Logger.LogDebug("ExtractData process has started.");
                var ms = await ExtractData();

                Logger.LogDebug("ParseData process has started.");
                var data = ParseData(ms);

                Logger.LogDebug("AnalyzeData process has started.");
                await AnalyzeData(data!);

                Logger.LogDebug("SaveData process has started.");
                await SaveData(data!);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "ExecuteAsync error");
            }
        }

        #endregion

        #region protected methods

        /// <summary>
        /// Extract the data from FileUploadRequest
        /// </summary>
        /// <returns>Task<MemoryStream></returns>
        protected async Task<MemoryStream> ExtractData()
        {
            var ms = new MemoryStream();
            await Request.File?.CopyToAsync(ms)!;
            return ms;
        }

        /// <summary>
        /// Parse data from MemoryStream
        /// </summary>
        /// <param name="stream">MemoryStream</param>
        /// <returns>string</returns>
        protected abstract string? ParseData(MemoryStream stream);

        /// <summary>
        /// Analyze the data, in case you need some validations within the system
        /// </summary>
        /// <param name="data">String</param>
        /// <returns>Task</returns>
        protected abstract Task AnalyzeData(string data);

        #endregion


        #region private methods, that needs to be hidden and is the same for all

        /// <summary>
        /// Save the data
        /// </summary>
        /// <param name="data">String</param>
        /// <returns>Task</returns>
        private async Task SaveData(string data)
        {
            var file = new FileData
            {
                Data = data,
                Username = _userRequest.Username,
                Description = Request.Description,
                Name = Request.Name,
                LastUpdate = DateTime.UtcNow
            };

            await _repository.SaveOrUpdateAsync(file);
            Logger.LogDebug("SaveData process finished successfully.");
        }
        #endregion
    }
}
