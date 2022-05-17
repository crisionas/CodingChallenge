using CC.IdentityService.Interfaces;
using CC.IdentityService.Models.Settings;
using CC.IdentityService.Repository;

namespace CC.IdentityService.Infrastructure
{
    public static class ServicesDependenciesBinder
    {
        public static void BindServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<IdentitySettings>(configuration.GetSection(IdentitySettings.SectionName));

            //Singleton services
            services.AddSingleton<IAuthRepository, AuthRepository>();
            services.AddAutoMapper(typeof(MappingSetup));
        }
    }
}
