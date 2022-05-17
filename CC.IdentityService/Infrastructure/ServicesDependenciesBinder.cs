using CC.IdentityService.Models.Settings;

namespace CC.IdentityService.Infrastructure
{
    public static class ServicesDependenciesBinder
    {
        public static void BindServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<IdentitySettings>(configuration.GetSection(IdentitySettings.SectionName));

        }
    }
}
