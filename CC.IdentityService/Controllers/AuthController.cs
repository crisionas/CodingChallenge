﻿using CC.Common;
using CC.Common.Models;
using CC.IdentityService.Interfaces;
using CC.IdentityService.Models.Requests;
using CC.IdentityService.Models.Responses;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CC.IdentityService.Controllers
{
    [Authorize]
    [Route("auth")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IValidator<RegisterRequest> _registerValidator;
        private readonly IValidator<AuthRequest> _authValidator;

        private readonly IAuthWorker _authWorker;

        public AuthController(IAuthWorker authWorker, IValidator<RegisterRequest> registerValidator, IValidator<AuthRequest> authValidator)
        {
            _authWorker = authWorker;
            _registerValidator = registerValidator;
            _authValidator = authValidator;
        }

        [SwaggerOperation(Summary = "Test method for checking authentication process.")]
        [HttpGet("test")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(401, Type = typeof(void))]
        public IActionResult Test()
        {
            return Ok("Authenticated successfully.");
        }
        
        [SwaggerOperation(Summary = "Register process, to access UploadService need to use its scope, available scopes: identity.scope, and upload.scope.")]
        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(204, Type = typeof(void))]
        [ProducesResponseType(400, Type = typeof(BaseResponse))]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var validationResult = await _registerValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return GetResponseFromValidationResult(validationResult);

            var response = await _authWorker.RegistrationAsync(request);
            return PrepareNoContentResult(response);
        }

        [SwaggerOperation(Summary = "Authentication Api. Copy token and access service.")]
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(200, Type = typeof(AuthResponse))]
        [ProducesResponseType(400, Type = typeof(AuthResponse))]
        public async Task<IActionResult> Login(AuthRequest request)
        {
            var validationResult = await _authValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return GetResponseFromValidationResult(validationResult);

            var response = await _authWorker.AuthenticateAsync(request);
            return PrepareActionResult(response);
        }
    }
}
