using CC.Common;
using CC.UploadService.Interfaces;
using CC.UploadService.Repository;
using CC.UploadService.Workers;
using FluentValidation;
using Hangfire;
using Hangfire.MemoryStorage;
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
            services.AddScoped<IFileUploaderWorker, FileUploaderWorker>();
            services.AddScoped<IBackgroundJobClient, BackgroundJobClient>();
            services.AddScoped<ITrackWorker, TrackWorker>();
            services.AddScoped<IEmailSenderWorker, EmailSenderWorker>();

            //Add Fluent validation DJ
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            //Add HangFire DJ
            services.AddHangfire(c => c.UseMemoryStorage());
            services.AddHangfireServer();
        }
    }
}
