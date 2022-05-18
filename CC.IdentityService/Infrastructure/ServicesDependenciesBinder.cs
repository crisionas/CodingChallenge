using CC.Common;
using CC.IdentityService.Interfaces;
using CC.IdentityService.Models.Settings;
using CC.IdentityService.Repository;
using CC.IdentityService.Workers;

namespace CC.IdentityService.Infrastructure
{
    public static class ServicesDependenciesBinder
    {
        public static void BindServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add Authentication
            services.AddIdentityAuthentication(configuration);

            // Add Options
            services.Configure<IdentitySettings>(configuration.GetSection(IdentitySettings.SectionName));

            //Scoped services
            services.AddScoped<IAuthWorker, AuthWorker>();

            //Singleton services
            services.AddSingleton<IAuthRepository, AuthRepository>();
            services.AddAutoMapper(typeof(MappingSetup));
        }
    }
}
