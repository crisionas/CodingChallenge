using AutoMapper;
using CC.Common;
using CC.Common.Models;
using CC.IdentityService.Interfaces;
using CC.IdentityService.Models.Requests;
using CC.IdentityService.Models.Responses;
using CC.IdentityService.Models.Settings;
using CC.IdentityService.Repository.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CC.IdentityService.Workers
{
    public class AuthWorker : BaseWorker, IAuthWorker
    {
        private readonly IAuthRepository _repository;
        private readonly IMapper _mapper;
        private readonly IdentitySettings _settings;

        public AuthWorker(ILogger<AuthWorker> logger, IMapper mapper, IAuthRepository repository, IOptions<IdentitySettings> options) : base(logger)
        {
            _repository = repository;
            _settings = options.Value;
            _mapper = mapper;
        }

        #region public methods

        public async Task<BaseResponse> RegistrationAsync(RegisterRequest request)
        {
            var response = new BaseResponse();
            try
            {
                var model = _mapper.Map<User>(request);
                await _repository.SaveUserAsync(model);
            }
            catch (Exception ex)
            {
                WriteBaseResponse(response, ex, "RegistrationAsync error");
            }

            return response;
        }

        public async Task<AuthResponse> AuthenticateAsync(AuthRequest request)
        {
            var response = new AuthResponse();
            try
            {
                var result = await _repository.GetUserAsync(request.Username!);
                if (result == null || result.Password != request.Password)
                    throw new Exception("Incorrect username or password");

                response.Token = BuildToken(result);
            }
            catch (Exception e)
            {
                WriteBaseResponse(response, e, "AuthenticateAsync error");
            }

            return response;
        }
        #endregion

        #region private methods

        /// <summary>
        /// Build Jwt token based on user info
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>JWT Token</returns>
        private string BuildToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_settings.Secret);

            var authCred = user.Scopes.Join(_settings.AuthCredentials, x => x,
                g => g.Scope,
                (x, g) => new { Cred = g, Scope = x })
                .Where(x => x.Scope == x.Cred.Scope).Select(x => x.Cred).ToList();
            var userScopes = authCred.Select(x => new Claim("scope", x.Scope!));

            var scopes = new List<Claim>
            {
                new(ClaimTypes.Role, "user"),
                new(ClaimTypes.Name, user.Username!),
                new("id", user.Id!),
                new("company", user.Company!)
            };
            scopes.AddRange(userScopes);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(scopes),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _settings.Issuer,
                Audience = string.Join(" ", authCred.Select(x => x.Audience))
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        #endregion

    }
}
