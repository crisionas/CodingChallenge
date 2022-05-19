using CC.UploadService.Models.Requests;

namespace CC.UploadService.Interfaces
{
    public interface IDataMiner
    {
        /// <summary>
        /// Execute the processor
        /// </summary>
        /// <param name="request">FileUploadRequest</param>
        /// <returns>Task</returns>
        Task<string?> ExecuteAsync(FileUploadRequest request);
    }
}
