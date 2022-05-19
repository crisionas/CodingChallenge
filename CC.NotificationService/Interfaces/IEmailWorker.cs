using CC.Common.Models;

namespace CC.NotificationService.Interfaces
{
    public interface IEmailWorker
    {
        /// <summary>
        /// Send mail async
        /// </summary>
        /// <param name="message">EmailMessage</param>
        /// <returns>Task of BaseResponse</returns>
        Task<BaseResponse> SendMailAsync(EmailMessage message);
    }
}
