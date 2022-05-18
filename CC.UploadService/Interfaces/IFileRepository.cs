using CC.UploadService.Repository.Entities;

namespace CC.UploadService.Interfaces
{
    public interface IFileRepository
    {
        Task<FileData?> GetAsync(string fileId);
        Task SaveOrUpdateAsync(FileData file);
    }
}
