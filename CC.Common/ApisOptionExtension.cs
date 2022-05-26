using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace CC.Common
{
    public static class ApisOptionExtension
    {
        public static void AddFluentValidationBaseResponse(this IServiceCollection services, Action<FluentValidationMvcConfiguration> action)
        {
            services.AddControllers(options =>
                {
                    options.Filters.Add(typeof(ValidateModelStateAttribute));
                })
                .AddFluentValidation(action);

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        }
    }
}
