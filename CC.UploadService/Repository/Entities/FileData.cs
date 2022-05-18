namespace CC.UploadService.Repository.Entities
{
    public class FileData
    {
        public string FileId { get; set; } = Guid.NewGuid().ToString();
        public string? Name { get; set; }
        public string? Description { get; set; }
        public FileStream? FileStream { get; set; }
    }
}
