using Microsoft.Extensions.DependencyInjection;
using Monkey.Filters.Authorize;
using Monkey.Filters.Exception;
using Monkey.Filters.ModelValidation;

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
            // Filter
            services.AddScoped<ApiExceptionFilter>();
            services.AddScoped<ApiAuthorizeActionFilter>();
            services.AddScoped<ApiModelValidationActionFilter>();
            return services;
        }
    }
}