using CC.Common.Models;

namespace CC.UploadService.Interfaces
{
    public interface IEmailSenderWorker
    {
        Task SendNotification(EmailMessage message);
    }
}
