using AutoMapper;
using CC.IdentityService.Controllers;
using CC.IdentityService.Models;
using CC.IdentityService.Models.Requests;
using CC.IdentityService.Models.Settings;
using CC.IdentityService.Repository;
using CC.IdentityService.Repository.Entities;
using CC.IdentityService.Validators;
using CC.IdentityService.Workers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;

namespace CC.IdentityServiceTests
{
    public class BaseTests
    {
        protected AuthController AuthController = null!;
        private IOptions<IdentitySettings> _settingsOptions = null!;

        public BaseTests()
        {
            InitOptions();
            InitAuthController();
        }

        #region private methods

        private void InitAuthController()
        {
            var authWorker = new AuthWorker(CreateFakeLogger<AuthWorker>(), CreateAutoMapper(),
                CreateMoqObject<AuthRepository>(), _settingsOptions);
            AuthController = new AuthController(authWorker, new RegisterRequestValidator(_settingsOptions),
                new AuthRequestValidator());
        }

        private void InitOptions()
        {
            _settingsOptions = Options.Create(new IdentitySettings
            {
                Secret = "Secret for test cases",
                Issuer = "http://test:5201",
                AuthCredentials = new List<AuthCredentials>
                {
                    new()
                    {
                        Audience = "notify.api",
                        Scope = "notify.scope"
                    },
                    new()
                    {
                        Audience = "identity.api",
                        Scope="identity.scope"
                    }
                }
            });
        }

        #endregion

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

        /// <summary>
        /// Create AutoMapper Config
        /// </summary>
        /// <returns>IMapper</returns>
        protected static IMapper CreateAutoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RegisterRequest, User>()
                    .ForMember(dest => dest.Id,
                        opt => opt.MapFrom(src => Guid.NewGuid()));
            });
            return config.CreateMapper();
        }

        #endregion
    }
}
