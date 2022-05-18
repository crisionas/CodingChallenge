using CC.Common;
using CC.UploadService.Interfaces;
using CC.UploadService.Repository;
using FluentValidation;
using System.Reflection;

namespace CC.UploadService.Infrastructure
{
    public static class ServicesDependenciesBinder
    {
        public static void BindServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Add Singleton
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IFileRepository, FileRepository>();

            //Add Authentication
            services.AddIdentityAuthentication(configuration);

            //Add Scoped
            services.AddScoped<IUserRequestSettings, UserRequestSettings>();

            //Add Fluent validation DJ
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
