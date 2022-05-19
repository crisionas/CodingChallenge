using CC.Common;
using CC.Common.Models;
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
        private readonly IEmailSenderWorker _emailWorker;

        protected FileUploadRequest Request = null!;

        protected DataMiner(ILogger logger, IFileRepository repository, IUserRequestSettings userRequest,
            IBackgroundJobClient jobClient, IEmailSenderWorker emailWorker)
        {
            _repository = repository;
            _userRequest = userRequest;
            Logger = logger;
            _jobClient = jobClient;
            _emailWorker = emailWorker;
        }

        #region public methods

        protected string? ParsedString;

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
                var byteArray = await ExtractData();

                Logger.LogDebug("ParseData process has started.");
                var trackId = _jobClient.Enqueue(() => ParseData(byteArray));
                await SendMailAsync(_userRequest.Email!, $"Your file has started processing. Tracking Id: {trackId}");

                var fileData = AttachFileData(trackId);

                Logger.LogDebug("AnalyzeData process has started.");
                BackgroundJob.ContinueJobWith(trackId, () => AnalyzeData());

                Logger.LogDebug("SaveData process has started.");
                BackgroundJob.ContinueJobWith(trackId, () => SaveData(fileData));

                BackgroundJob.ContinueJobWith(trackId, () => SendResult(true, null, _userRequest.Email));
            }
            catch (Exception e)
            {
                Logger.LogError(e, "ExecuteAsync error");
                await SendResult(false, e.Message);
            }
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

        /// <summary>
        /// Send the final result
        /// </summary>
        /// <param name="isSuccessful">The process success</param>
        /// <param name="errorMessage">Error message</param>
        /// <param name="email">Email is necessary to be explicit within Hangfire jobs</param>
        /// <returns></returns>
        public async Task SendResult(bool isSuccessful = true, string? errorMessage = null, string? email = null)
        {
            var message = isSuccessful
                ? "The file was uploaded successful."
                : $"The file upload failed. Error:{errorMessage}";
            await SendMailAsync(_userRequest.Email! ?? email!, message);
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

        private async Task SendMailAsync(string email, string message)
        {
            if (string.IsNullOrEmpty(email))
                return;
            var emailMessage = new EmailMessage
            {
                Email = email,
                Subject = "Uploading file process",
                Message = message
            };
            await _emailWorker.SendNotification(emailMessage);
        }
        #endregion
    }
}
