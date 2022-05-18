using CC.UploadService.Interfaces;
using CC.UploadService.Repository.Entities;
using System.Collections.Concurrent;

namespace CC.UploadService.Repository
{
    public class FileRepository : IFileRepository
    {
        private static readonly ConcurrentBag<FileData> Db = new();

        public async Task<FileData?> GetAsync(string fileId)
        {
            return await Task.Run(() => Db.FirstOrDefault(x => x.FileId == fileId));
        }

        public async Task SaveOrUpdateAsync(FileData file)
        {
            await Task.Run(() => Db.Add(file));
        }
    }
}
