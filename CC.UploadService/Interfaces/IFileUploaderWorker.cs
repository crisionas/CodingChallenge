using CC.UploadService.Models.Requests;

namespace CC.UploadService.Interfaces
{
    public interface IFileUploaderWorker
    {
        /// <summary>
        /// Upload file async
        /// </summary>
        /// <param name="request">FileUploadRequest</param>
        /// <returns>Task</returns>
        Task UploadFileAsync(FileUploadRequest request);
    }
}
