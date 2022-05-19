using CC.Common;
using CC.Common.Models;
using CC.UploadService.Interfaces;
using Newtonsoft.Json;
using System.Text;

namespace CC.UploadService.Workers
{
    public class EmailSenderWorker : BaseWorker, IEmailSenderWorker
    {
        public EmailSenderWorker(ILogger<EmailSenderWorker> logger) : base(logger)
        {
        }

        public async Task SendNotification(EmailMessage message)
        {
            try
            {
                using var client = new HttpClient();
                var json = JsonConvert.SerializeObject(message);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://localhost:7081/notification", data);

                if (!response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    throw new Exception(result);
                }
            }
            catch (Exception e)
            {
                WriteBaseResponse(null, e, "SendToNotification error");
            }
        }
    }
}
