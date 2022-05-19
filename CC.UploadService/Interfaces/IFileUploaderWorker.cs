using CC.UploadService.Models.Requests;
using CC.UploadService.Models.Responses;

namespace CC.UploadService.Interfaces
{
    public interface IFileUploaderWorker
    {
        /// <summary>
        /// Upload file async
        /// </summary>
        /// <param name="request">FileUploadRequest</param>
        /// <returns>Task</returns>
        Task<FileUploadResponse> UploadFileAsync(FileUploadRequest request);
    }
}
