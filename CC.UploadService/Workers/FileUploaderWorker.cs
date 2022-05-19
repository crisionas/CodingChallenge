using CC.Common;
using CC.UploadService.DataMiners;
using CC.UploadService.Helpers;
using CC.UploadService.Interfaces;
using CC.UploadService.Models.Requests;
using Hangfire;

namespace CC.UploadService.Workers
{
    public class FileUploaderWorker : BaseWorker, IFileUploaderWorker
    {
        private readonly IUserRequestSettings _requestSettings;
        private readonly IFileRepository _fileRepository;
        private readonly IBackgroundJobClient _jobClient;
        private readonly IEmailSenderWorker _emailSenderWorker;

        public FileUploaderWorker(ILogger<FileUploaderWorker> logger, IUserRequestSettings requestSettings,
            IFileRepository fileRepository, IBackgroundJobClient jobClient, IEmailSenderWorker emailSenderWorker) : base(logger)
        {
            _requestSettings = requestSettings;
            _fileRepository = fileRepository;
            _jobClient = jobClient;
            _emailSenderWorker = emailSenderWorker;
        }

        public async Task UploadFileAsync(FileUploadRequest request)
        {
            try
            {
                if (request.File == null)
                    throw new Exception("File is null");

                var dataMiner = SelectDataMiner(request.File.ContentType);

                Logger.LogDebug($"File with main name {request.Name}, {request.File.Name} has started processing.");

                await dataMiner?.ExecuteAsync(request)!;
            }
            catch (Exception e)
            {
                WriteBaseResponse(null, e, "UploadFileAsync error");
            }
        }

        #region private methods

        private IDataMiner? SelectDataMiner(string fileType)
        {
            if (string.Equals(fileType, FileFormats.PdfFormat))
                return new PdfDataMiner(GetLogger<PdfDataMiner>(), _fileRepository, _requestSettings, _jobClient, _emailSenderWorker);
            if (string.Equals(fileType, FileFormats.CsvFormat))
                return new CsvDataMiner(GetLogger<CsvDataMiner>(), _fileRepository, _requestSettings, _jobClient, _emailSenderWorker);
            throw new Exception($"File type {fileType} is not supported.");
        }

        #endregion
    }
}
