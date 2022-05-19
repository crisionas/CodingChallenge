using CC.Common;
using CC.UploadService.Controllers;
using CC.UploadService.Repository;
using CC.UploadService.Validators;
using CC.UploadService.Workers;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;

namespace CC.UploadServiceTests
{
    public class TestBase
    {
        protected UploadController UploadController;
        protected TrackController TrackController;
        public TestBase()
        {
            GenerateControllers();
        }

        #region protected methods
        /// <summary>
        /// Create Fake Logger
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <returns>ILogger</returns>
        protected static ILogger<T> CreateFakeLogger<T>()
        {
            var mockLogger = new Mock<ILogger<T>>();
            return mockLogger.Object;
        }

        /// <summary>
        /// Create Moq Object
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <returns>Object</returns>
        protected static T CreateMoqObject<T>() where T : class
        {
            var mockLogger = new Mock<T>();
            return mockLogger.Object;
        }

        #endregion

        #region private methods

        private void GenerateControllers()
        {
            var claimsObj = new Mock<ClaimsPrincipal>();
            claimsObj.Setup(x => x.Claims).Returns(new Claim[]
            {
                new(ClaimTypes.Name, "TestName"),
                new("company", "TestCompany"),
                new(ClaimTypes.Email, "TestName@gmail.com")
            });

            var httpContext = new HttpContextAccessor()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = claimsObj.Object
                }
            };

            var userRequest = new UserRequestSettings(httpContext);
            var fileUploadWorker = new FileUploaderWorker(CreateFakeLogger<FileUploaderWorker>(), userRequest,
                CreateMoqObject<FileRepository>(), new BackgroundJobClient(new MemoryStorage()), new EmailSenderWorker(CreateFakeLogger<EmailSenderWorker>()));

            UploadController = new UploadController(new FileUploadRequestValidator(), fileUploadWorker);
        }

        #endregion
    }
}
