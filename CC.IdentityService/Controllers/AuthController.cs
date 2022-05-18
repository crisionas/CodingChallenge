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
    public class AuthController :ControllerBase
    {
        private readonly IValidator<RegisterRequest> _registerValidator;

        private readonly IAuthWorker _authWorker;

        public AuthController(IAuthWorker authWorker, IValidator<RegisterRequest> registerValidator)
        {
            _authWorker = authWorker;
            _registerValidator = registerValidator;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var validationResult=await _registerValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult);
            var response = await _authWorker.RegistrationAsync(request);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthRequest request)
        {
            var response = await _authWorker.AuthenticateAsync(request);
            return Ok(response);
        }

        [HttpPost("test")]
        public IActionResult Test(AuthRequest request)
        {
            return Ok("test");
        }
    }
}
