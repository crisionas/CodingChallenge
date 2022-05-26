using CC.Common;
using CC.Common.Models;
using CC.NotificationService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CC.NotificationService.Controllers
{
    [Route("notification")]
    public class NotificationController : BaseController
    {
        private readonly IEmailWorker _emailWorker;

        public NotificationController(IEmailWorker emailWorker)
        {
            _emailWorker = emailWorker;
        }

        [SwaggerOperation(Summary = "Send email notification to a specific email.")]
        [HttpPost]
        [ProducesResponseType(204, Type = typeof(void))]
        [ProducesResponseType(400, Type = typeof(BaseResponse))]
        public async Task<IActionResult> SendNotification(EmailMessage message)
        {
            var response = await _emailWorker.SendMailAsync(message);
            return PrepareNoContentResult(response);
        }
    }
}
