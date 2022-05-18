using CC.Common.Models;
using CC.IdentityService.Models.Requests;
using CC.IdentityService.Models.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CC.IdentityServiceTests.AuthTests
{
    [TestClass]
    public class AuthenticationTests : BaseTests
    {
        [TestMethod]
        public async Task AuthShouldWork()
        {
            //Register users in memory
            await RegisterUsers();

            //------------------------User1------------------
            var request = new AuthRequest
            {
                Username = "TestUser1",
                Password = "myPassword"
            };

            var result = await AuthController.Login(request);
            var objectResult = result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(StatusCodes.Status200OK, objectResult.StatusCode);
            var results = objectResult.Value;
            Assert.IsInstanceOfType(results, typeof(BaseResponse));
            var response = results as AuthResponse;
            Assert.IsNotNull(response);
            Assert.IsFalse(string.IsNullOrEmpty(response.Token));
            Assert.IsFalse(response.HasError);
            Assert.IsTrue(string.IsNullOrEmpty(response.ErrorMessage));

            //------------------------User2------------------
            request = new AuthRequest
            {
                Username = "TestUser2",
                Password = "myPassword"
            };

            result = await AuthController.Login(request);
            objectResult = result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(StatusCodes.Status200OK, objectResult.StatusCode);
            results = objectResult.Value;
            Assert.IsInstanceOfType(results, typeof(BaseResponse));
            response = results as AuthResponse;
            Assert.IsNotNull(response);
            Assert.IsFalse(string.IsNullOrEmpty(response.Token));
            Assert.IsFalse(response.HasError);
            Assert.IsTrue(string.IsNullOrEmpty(response.ErrorMessage));
        }

        [TestMethod]
        public async Task AuthShouldFailInvalidProperties()
        {
            //Invalid Username
            var request = new AuthRequest
            {
                Username = string.Empty,
                Password = "myPassword"
            };

            var result = await AuthController.Login(request);
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
            request = new AuthRequest
            {
                Username = "TestUser2"
            };

            result = await AuthController.Login(request);
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
        public async Task AuthShouldFailUserDoesntExist()
        {
            await RegisterUsers();

            //Invalid user
            var request = new AuthRequest
            {
                Username = "NotExists",
                Password = "NotExists"
            };

            var result = await AuthController.Login(request);
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

        #region private methods

        private async Task RegisterUsers()
        {
            var request = new RegisterRequest
            {
                Username = "TestUser1",
                Password = "myPassword",
                Company = "TestCompany",
                Scopes = new List<string> { "notify.scope" }
            };
            await AuthController.Register(request);

            request.Username = "TestUser2";
            await AuthController.Register(request);
        }

        #endregion
    }
}
