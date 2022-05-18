using CC.Common;

namespace CC.UploadService.Infrastructure
{
    public static class ServicesDependenciesBinder
    {
        public static void BindServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Add Authentication
            services.AddIdentityAuthentication(configuration);
        }
    }
}
