using CC.Common;
using CC.Common.Models;
using CC.NotificationService.Interfaces;
using CC.NotificationService.Models;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace CC.NotificationService.Workers
{
    public class EmailWorker : BaseWorker, IEmailWorker
    {
        private readonly SmtpSettings _settings;


        public EmailWorker(ILogger<EmailWorker> logger, IOptions<SmtpSettings> options) : base(logger)
        {
            _settings = options.Value;
        }

        /// <summary>
        /// Send mail async
        /// </summary>
        /// <param name="message">EmailMessage</param>
        /// <returns>Task of BaseResponse</returns>
        public async Task<BaseResponse> SendMailAsync(EmailMessage message)
        {
            var response = new BaseResponse();
            try
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_settings.Username, _settings.DisplayName),
                    Priority = MailPriority.High,
                    To = { message.Email! },
                    Subject = message.Subject,
                    Body = message.Message
                };

                var client = new SmtpClient(_settings.Host, _settings.Port);

                client.Credentials = new NetworkCredential(_settings.Username, _settings.Password);
                client.EnableSsl = _settings.EnableSsl;

                await client.SendMailAsync(mailMessage);

                Logger.LogDebug($"SendMailAsync | The message for the address {mailMessage.To} was successfully sent");
            }
            catch (Exception e)
            {
                WriteBaseResponse(response, e, "SendMailAsync error");
            }
            return response;
        }
    }
}
