namespace CC.UploadService.Models.Requests
{
    public class FileUploadRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? File { get; set; }
    }
}
