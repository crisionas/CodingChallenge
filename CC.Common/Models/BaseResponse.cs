namespace CC.Common.Models
{
    public class BaseResponse
    {
        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
        public string? ErrorMessage { get; set; }
    }
}
