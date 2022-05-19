using CC.UploadService.Repository.Entities;

namespace CC.UploadService.Interfaces
{
    public interface IFileRepository
    {
        /// <summary>
        /// Get file by id
        /// </summary>
        /// <param name="fileId">FileId</param>
        /// <returns>FileData</returns>
        Task<FileData?> GetAsync(string fileId);

        /// <summary>
        /// Save or Update the file
        /// </summary>
        /// <param name="file">FileData</param>
        /// <returns>Task</returns>
        Task SaveOrUpdateAsync(FileData file);
    }
}
