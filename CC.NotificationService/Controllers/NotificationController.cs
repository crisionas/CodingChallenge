﻿using CC.Common;
using CC.Common.Models;
using CC.NotificationService.Interfaces;
using CC.NotificationService.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CC.NotificationService.Controllers
{
    [Route("notification")]
    [ApiController]
    public class NotificationController : BaseController
    {
        private readonly IEmailWorker _emailWorker;
        private readonly IValidator<EmailMessage> _emailValidator;

        public NotificationController(IEmailWorker emailWorker, IValidator<EmailMessage> emailValidator)
        {
            _emailWorker = emailWorker;
            _emailValidator = emailValidator;
        }

        [HttpPost]
        [ProducesResponseType(204, Type = typeof(void))]
        [ProducesResponseType(400, Type = typeof(BaseResponse))]
        public async Task<IActionResult> SendNotification(EmailMessage message)
        {
            var validationResult = await _emailValidator.ValidateAsync(message);
            if (!validationResult.IsValid)
                return GetResponseFromValidationResult(validationResult);

            var response = await _emailWorker.SendMailAsync(message);
            return PrepareNoContentResult(response);
        }
    }
}