using CC.Common.Models;
using CC.IdentityService.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CC.IdentityServiceTests.AuthTests
{
    [TestClass]
    public class RegistrationTests : BaseTests
    {
        [TestMethod]
        public async Task RegistrationShouldWork()
        {
            //------------------------User1------------------
            var request = new RegisterRequest
            {
                Username = "TestUser1",
                Password = "myPassword",
                Company = "MyCompany",
                Scopes = new List<string> { "notify.scope" }
            };

            var result = await AuthController.Register(request);
            var okResult = result as NoContentResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(StatusCodes.Status204NoContent, okResult.StatusCode);

            //------------------------User2------------------
            request = new RegisterRequest
            {
                Username = "TestUser2",
                Password = "myPassword",
                Company = "MyCompany",
                Scopes = new List<string> { "identity.scope" }
            };

            result = await AuthController.Register(request);
            okResult = result as NoContentResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(StatusCodes.Status204NoContent, okResult.StatusCode);
        }

        [TestMethod]
        public async Task RegistrationShouldFailInvalidScopes()
        {
            //------------------------User1------------------
            var request = new RegisterRequest
            {
                Username = "TestUser1",
                Password = "myPassword",
                Company = "MyCompany",
                Scopes = new List<string> { "test1.scope" }
            };

            var result = await AuthController.Register(request);
            var badResult = result as ObjectResult;
            Assert.IsNotNull(badResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, badResult.StatusCode);
            var results = badResult.Value;
            Assert.IsInstanceOfType(results, typeof(BaseResponse));
            var response = results as BaseResponse;
            Assert.IsNotNull(response);
            Assert.IsTrue(response.HasError);
            Assert.IsFalse(string.IsNullOrEmpty(response.ErrorMessage));


            //------------------------User2------------------
            request = new RegisterRequest
            {
                Username = "TestUser2",
                Password = "myPassword",
                Company = "MyCompany",
                Scopes = new List<string> { "test2.scope" }
            };

            result = await AuthController.Register(request);
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
        public async Task RegistrationShouldFailDuplicate()
        {
            //First attempt
            var request = new RegisterRequest
            {
                Username = "TestUser1",
                Password = "myPassword",
                Company = "MyCompany",
                Scopes = new List<string> { "notify.scope" }
            };
            await AuthController.Register(request);

            //Second attempt
            request = new RegisterRequest
            {
                Username = "TestUser1",
                Password = "myPassword",
                Company = "MyCompany",
                Scopes = new List<string> { "notify.scope" }
            };

            var result = await AuthController.Register(request);
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
        public async Task RegistrationShouldFailInvalidData()
        {
            //Invalid Username
            var request = new RegisterRequest
            {
                Username = string.Empty,
                Password = "myPassword",
                Company = "MyCompany",
                Scopes = new List<string> { "notify.scope" }
            };

            var result = await AuthController.Register(request);
            var badResult = result as ObjectResult;
            Assert.IsNotNull(badResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, badResult.StatusCode);
            var results = badResult.Value;
            Assert.IsInstanceOfType(results, typeof(BaseResponse));
            var response = results as BaseResponse;
            Assert.IsNotNull(response);
            Assert.IsTrue(response.HasError);
            Assert.IsFalse(string.IsNullOrEmpty(response.ErrorMessage));


            //Invalid Password
            request = new RegisterRequest
            {
                Username = "TestUser2",
                Password = null,
                Company = "MyCompany",
                Scopes = new List<string> { "notify.scope" }
            };

            result = await AuthController.Register(request);
            badResult = result as ObjectResult;
            Assert.IsNotNull(badResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, badResult.StatusCode);
            results = badResult.Value;
            Assert.IsInstanceOfType(results, typeof(BaseResponse));
            response = results as BaseResponse;
            Assert.IsNotNull(response);
            Assert.IsTrue(response.HasError);
            Assert.IsFalse(string.IsNullOrEmpty(response.ErrorMessage));

            //Company not filled
            request = new RegisterRequest
            {
                Username = "TestUser2",
                Password = null,
                Scopes = new List<string> { "notify.scope" }
            };

            result = await AuthController.Register(request);
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
    }
}
