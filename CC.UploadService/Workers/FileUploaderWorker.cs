using CC.Common;
using CC.UploadService.DataMiners;
using CC.UploadService.Helpers;
using CC.UploadService.Interfaces;
using CC.UploadService.Models.Requests;
using CC.UploadService.Models.Responses;
using Hangfire;

namespace CC.UploadService.Workers
{
    public class FileUploaderWorker : BaseWorker, IFileUploaderWorker
    {
        private readonly IUserRequestSettings _requestSettings;
        private readonly IFileRepository _fileRepository;
        private readonly IBackgroundJobClient _jobClient;

        public FileUploaderWorker(ILogger<FileUploaderWorker> logger, IUserRequestSettings requestSettings, IFileRepository fileRepository, IBackgroundJobClient jobClient) : base(logger)
        {
            _requestSettings = requestSettings;
            _fileRepository = fileRepository;
            _jobClient = jobClient;
        }

        public async Task<FileUploadResponse> UploadFileAsync(FileUploadRequest request)
        {
            var response = new FileUploadResponse();
            try
            {
                if (request.File == null)
                    throw new Exception("File is null");

                var dataMiner = SelectDataMiner(request.File.ContentType);

                Logger.LogDebug($"File with main name {request.Name}, {request.File.Name} has started processing.");

                response.TrackId = await dataMiner?.ExecuteAsync(request)!;
            }
            catch (Exception e)
            {
                WriteBaseResponse(response, e, "UploadFileAsync error");
            }

            return response;
        }

        #region private methods

        private IDataMiner? SelectDataMiner(string fileType)
        {
            if (string.Equals(fileType, FileFormats.PdfFormat))
                return new PdfDataMiner(GetLogger<PdfDataMiner>(), _fileRepository, _requestSettings, _jobClient);
            if (string.Equals(fileType, FileFormats.CsvFormat))
                return new CsvDataMiner(GetLogger<CsvDataMiner>(), _fileRepository, _requestSettings, _jobClient);
            throw new Exception($"File type {fileType} is not supported.");
        }

        #endregion
    }
}
