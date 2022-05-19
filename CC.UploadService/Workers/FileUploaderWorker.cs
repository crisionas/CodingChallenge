using CC.Common;
using CC.UploadService.DataMiners;
using CC.UploadService.Helpers;
using CC.UploadService.Interfaces;
using CC.UploadService.Models.Requests;

namespace CC.UploadService.Workers
{
    public class FileUploaderWorker : BaseWorker, IFileUploaderWorker
    {
        private readonly IUserRequestSettings _requestSettings;
        private readonly IFileRepository _fileRepository;
        public FileUploaderWorker(ILogger<FileUploaderWorker> logger, IUserRequestSettings requestSettings, IFileRepository fileRepository) : base(logger)
        {
            _requestSettings = requestSettings;
            _fileRepository = fileRepository;
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
                Logger.LogError(e, "UploadFileAsync error");
            }
        }

        #region private methods

        private IDataMiner? SelectDataMiner(string fileType)
        {
            if (string.Equals(fileType, FileFormats.DocxFormat))
                return new DocxDataMiner(GetLogger<DocxDataMiner>(), _fileRepository, _requestSettings);
            if (string.Equals(fileType, FileFormats.CsvFormat))
                return new CsvDataMiner(GetLogger<CsvDataMiner>(), _fileRepository, _requestSettings);
            throw new Exception($"File type {fileType} is not supported.");
        }

        #endregion
    }
}
