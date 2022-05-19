using CC.Common.Models;
using CC.NotificationService.Controllers;
using CC.NotificationService.Models;
using CC.NotificationService.Validators;
using CC.NotificationService.Workers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace CC.NotificationServiceTests
{
    [TestClass]
    public class NotificationControllerTests
    {
        private readonly NotificationController _controller;
        public NotificationControllerTests()
        {
            var emailWorker = new EmailWorker(CreateFakeLogger<EmailWorker>(), CreateFakeSmtpSettings());
            _controller = new NotificationController(emailWorker, new EmailMessageValidator());
        }

        [TestMethod]
        public async Task SendNotificationShouldWork()
        {
            //------------------------Mail message 1------------------
            var request = new EmailMessage()
            {
                Email = "fekaci7215@cupbest.com",
                Subject = "Test case 1",
                Message = "Test message"
            };

            var result = await _controller.SendNotification(request);
            var okResult = result as NoContentResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(StatusCodes.Status204NoContent, okResult.StatusCode);

            //------------------------Mail message 2------------------
            request = new EmailMessage()
            {
                Email = "pahokih575@doerma.com",
                Subject = "Test case 1",
                Message = "Test message"
            };

            result = await _controller.SendNotification(request);
            okResult = result as NoContentResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(StatusCodes.Status204NoContent, okResult.StatusCode);
        }

        [TestMethod]
        public async Task SendNotificationFailIncorrectEmail()
        {
            var request = new EmailMessage()
            {
                Email = "test@",
                Subject = "Test case 1",
                Message = "Test message"
            };

            var result = await _controller.SendNotification(request);
            var badResult = result as ObjectResult;
            Assert.IsNotNull(badResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, badResult.StatusCode);
            var results = badResult.Value;
            Assert.IsInstanceOfType(results, typeof(BaseResponse));
            var response = results as BaseResponse;
            Assert.IsNotNull(response);
            Assert.IsTrue(response.HasError);
            Assert.IsFalse(string.IsNullOrEmpty(response.ErrorMessage));
        }

        [TestMethod]
        public async Task SendNotificationFailIncorrectSubjectMessage()
        {
            //------------------------Subject is empty------------------
            var request = new EmailMessage()
            {
                Email = "test@gmail.com",
                Subject = string.Empty,
                Message = "Test message"
            };

            var result = await _controller.SendNotification(request);
            var badResult = result as ObjectResult;
            Assert.IsNotNull(badResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, badResult.StatusCode);
            var results = badResult.Value;
            Assert.IsInstanceOfType(results, typeof(BaseResponse));
            var response = results as BaseResponse;
            Assert.IsNotNull(response);
            Assert.IsTrue(response.HasError);
            Assert.IsFalse(string.IsNullOrEmpty(response.ErrorMessage));

            //------------------------Message is Empty------------------
            request = new EmailMessage()
            {
                Email = "test@gmail.com",
                Subject = "Test",
                Message = string.Empty
            };

            result = await _controller.SendNotification(request);
            badResult = result as ObjectResult;
            Assert.IsNotNull(badResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, badResult.StatusCode);
            results = badResult.Value;
            Assert.IsInstanceOfType(results, typeof(BaseResponse));
            response = results as BaseResponse;
            Assert.IsNotNull(response);
            Assert.IsTrue(response.HasError);
            Assert.IsFalse(string.IsNullOrEmpty(response.ErrorMessage));
        }


        #region private methods
        /// <summary>
        /// Create Fake Logger
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <returns>ILogger</returns>
        private static ILogger<T> CreateFakeLogger<T>()
        {
            var mockLogger = new Mock<ILogger<T>>();
            return mockLogger.Object;
        }

        /// <summary>
        /// Create FakeSmtpSettings
        /// </summary>
        /// <returns></returns>
        private static IOptions<SmtpSettings> CreateFakeSmtpSettings()
        {
            return Options.Create(new SmtpSettings
            {
                Host = "smtp.gmail.com",
                Port = 587,
                DisplayName = "Code Challenge Tests",
                EnableSsl = true,
                Username = "codechallengeprojectnotify@gmail.com",
                Password = "MyPassword"
            });
        }

        #endregion
    }
}
