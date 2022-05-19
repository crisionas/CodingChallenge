using CC.Common.Models;
using CC.UploadService.Helpers;
using CC.UploadService.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Reflection;

namespace CC.UploadServiceTests
{
    [TestClass]
    public class UploadControllerTests : TestBase
    {
        [TestMethod]
        public async Task UploadShouldRun()
        {
            // PDF
            var fileMock = new Mock<IFormFile>();
            var physicalFile = new FileInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Content/test.pdf");
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(physicalFile.OpenRead());
            await writer.FlushAsync();
            ms.Position = 0;
            var fileName = physicalFile.Name;
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.ContentDisposition).Returns($"inline; filename={fileName}");
            fileMock.Setup(_ => _.ContentType).Returns(FileFormats.PdfFormat);
            var request = new FileUploadRequest
            {
                Name = "test",
                File = fileMock.Object

            };

            var result = await UploadController.UploadFile(request);
            var noContent = result as NoContentResult;
            Assert.IsNotNull(noContent);
            Assert.AreEqual(StatusCodes.Status204NoContent, noContent.StatusCode);

            //CSV
            fileMock = new Mock<IFormFile>();
            physicalFile = new FileInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Content/test.csv");
            ms = new MemoryStream();
            writer = new StreamWriter(ms);
            writer.Write(physicalFile.OpenRead());
            await writer.FlushAsync();
            ms.Position = 0;
            fileName = physicalFile.Name;
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.ContentDisposition).Returns($"inline; filename={fileName}");
            fileMock.Setup(_ => _.ContentType).Returns(FileFormats.CsvFormat);
            request = new FileUploadRequest
            {
                Name = "test",
                File = fileMock.Object

            };

            result = await UploadController.UploadFile(request);
            noContent = result as NoContentResult;
            Assert.IsNotNull(noContent);
            Assert.AreEqual(StatusCodes.Status204NoContent, noContent.StatusCode);
        }

        [TestMethod]
        public async Task UploadShouldFailIncorrectFileTypes()
        {
            // DOCX
            var fileMock = new Mock<IFormFile>();
            var physicalFile = new FileInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Content/test.pdf");
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(physicalFile.OpenRead());
            await writer.FlushAsync();
            ms.Position = 0;
            var fileName = physicalFile.Name;
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.ContentDisposition).Returns($"inline; filename={fileName}");
            fileMock.Setup(_ => _.ContentType).Returns("application/docx");
            var request = new FileUploadRequest
            {
                Name = "test",
                File = fileMock.Object

            };
            var result = await UploadController.UploadFile(request);
            var badResult = result as ObjectResult;
            Assert.IsNotNull(badResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, badResult.StatusCode);
            var results = badResult.Value;
            Assert.IsInstanceOfType(results, typeof(BaseResponse));
            var response = results as BaseResponse;
            Assert.IsNotNull(response);
            Assert.IsTrue(response.HasError);
            Assert.IsFalse(string.IsNullOrEmpty(response.ErrorMessage));

            //TXT
            fileMock = new Mock<IFormFile>();
            physicalFile = new FileInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Content/test.csv");
            ms = new MemoryStream();
            writer = new StreamWriter(ms);
            writer.Write(physicalFile.OpenRead());
            await writer.FlushAsync();
            ms.Position = 0;
            fileName = physicalFile.Name;
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.ContentDisposition).Returns($"inline; filename={fileName}");
            fileMock.Setup(_ => _.ContentType).Returns("application/txt");
            request = new FileUploadRequest
            {
                Name = "test",
                File = fileMock.Object

            };
            result = await UploadController.UploadFile(request);
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

        [TestMethod]
        public async Task UploadFileShoulFailIncorrectPrimaryName()
        {
            // PDF
            var fileMock = new Mock<IFormFile>();
            var physicalFile = new FileInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Content/test.pdf");
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(physicalFile.OpenRead());
            await writer.FlushAsync();
            ms.Position = 0;
            var fileName = physicalFile.Name;
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.ContentDisposition).Returns($"inline; filename={fileName}");
            fileMock.Setup(_ => _.ContentType).Returns(FileFormats.PdfFormat);
            var request = new FileUploadRequest
            {
                //Incorrect name
                Name = string.Empty,
                File = fileMock.Object

            };

            var result = await UploadController.UploadFile(request);
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
    }
}
