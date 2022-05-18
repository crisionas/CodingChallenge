using CC.Common.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CC.Common
{
    public static class IdentityExtension
    {
        private static string? _audience;

        /// <summary>
        /// Custom Identity authentication within local micro services
        /// </summary>
        /// <param name="services">IServiceCollection></param>
        /// <param name="configuration">IConfiguration</param>
        public static void AddIdentityAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetSection("AuthSettings").Get<AuthSettings>();

            _audience = settings.Audience;
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.Audience = x.Audience;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(settings.Secret!)),
                        ValidAudience = settings.Audience,
                        ValidIssuer = settings.Authority,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        AudienceValidator = DoValidation
                    };
                });
        }

        /// <summary>
        /// Custom validation for AudienceValidator to check if exist necessary audience
        /// </summary>
        /// <param name="audiences">List of Audiences incoming</param>
        /// <param name="securityToken">Token</param>
        /// <param name="validationParameters">TokenValidationParameters</param>
        /// <returns>bool</returns>
        private static bool DoValidation(IEnumerable<string> audiences, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            return audiences.Any(x => x.Contains(_audience!));
        }
    }
}
