using Microsoft.Extensions.DependencyInjection;
using Monkey.Filters;
using Monkey.Filters.Authorize;

namespace Monkey.Extensions
{
    public static class ApiExtensions
    {
        /// <summary>
        ///     [Api Filter] Model Validation, Global Exception Filer and Authorize Filter 
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddApiFilter(this IServiceCollection services)
        {
            services.AddScoped<ApiExceptionFilter>();
            services.AddScoped<ApiAuthorizeActionFilter>();
            services.AddScoped<ApiModelValidationActionFilter>();
            return services;
        }
    }
}