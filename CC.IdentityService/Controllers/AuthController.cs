using CC.Common;
using CC.IdentityService.Interfaces;
using CC.IdentityService.Models.Requests;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        //Should not be access without authentication 
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("Authenticated successfully.");
        }
        
        //TODO:This API is for generating users, available scopes: notify.scope, identity.scope and upload.scope
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var validationResult = await _registerValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return GetResponseFromValidationResult(validationResult);

            var response = await _authWorker.RegistrationAsync(request);
            return PrepareNoContentResult(response);
        }

        [AllowAnonymous]
        [HttpPost("login")]
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
