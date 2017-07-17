using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Monkey.Models;
using Puppy.Web.Swagger;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Monkey
{
    public partial class Startup
    {
        public static class Swagger
        {
            private static readonly string DocumentTitle = ConfigurationRoot.GetValue<string>("Developers:ApiDocumentTitle");

            private static readonly string DocumentName = ConfigurationRoot.GetValue<string>("Developers:ApiDocumentName");

            private static readonly string DocumentApiBaseUrl = ConfigurationRoot.GetValue<string>("Developers:ApiDocumentUrl") + DocumentName;

            private static readonly string DocumentJsonFileName = ConfigurationRoot.GetValue<string>("Developers:ApiDocumentJsonFile");

            private static readonly string DocumentUrlBase = DocumentApiBaseUrl.Replace(DocumentName, string.Empty).TrimEnd('/');

            private static readonly string SwaggerEndpoint = $"{DocumentUrlBase}/{DocumentName}/{DocumentJsonFileName}";

            public static void Service(IServiceCollection services)
            {
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc(DocumentName, new Info
                    {
                        Title = DocumentTitle,
                        Version = DocumentName,
                        Contact = new Contact
                        {
                            Name = "Top Nguyen",
                            Email = "TopNguyen92@gmail.com",
                            Url = "http://topnguyen.net"
                        }
                    });

                    var apiDocumentFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Monkey.xml");

                    options.IncludeXmlComments(apiDocumentFilePath);
                    options.DocumentFilter<HideInDocsFilter>();
                    options.IgnoreObsoleteProperties();
                    options.IgnoreObsoleteActions();
                    options.DescribeAllEnumsAsStrings();
                    options.DescribeAllParametersInCamelCase();
                });
            }

            public static void Middleware(IApplicationBuilder app)
            {
                app.UseSwagger(c =>
                {
                    c.RouteTemplate = $"{DocumentUrlBase.TrimStart('/')}/{{documentName}}/{DocumentJsonFileName}";
                    c.PreSerializeFilters.Add((swaggerDoc, httpReq) => swaggerDoc.Host = httpReq.Host.Value);
                });

                app.UseSwaggerUI(c =>
                {
                    c.RoutePrefix = DocumentApiBaseUrl.TrimStart('/');
                    string accessKey = ConfigurationRoot.GetValue<string>("Developers:AccessKey");
                    c.SwaggerEndpoint($"{SwaggerEndpoint}?key={accessKey}", DocumentTitle);
                });

                app.UseMiddleware<AccessMiddleware>();
            }

            public class AccessMiddleware
            {
                private readonly RequestDelegate _next;

                public AccessMiddleware(RequestDelegate next)
                {
                    _next = next;
                }

                public Task InvokeAsync(HttpContext context)
                {
                    if (IsSwaggerUi(context) || IsSwaggerEndpoint(context))
                        if (!IsCanAccessSwagger(context))
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            return Task.FromResult(0);
                        }

                    return _next.Invoke(context);
                }

                private static bool IsSwaggerUi(HttpContext httpContext)
                {
                    var pathQuery = httpContext.Request.Path.Value?.Trim('/').ToLower() ?? string.Empty;
                    var documentApiBaseUrl = DocumentApiBaseUrl.Trim('/') ?? string.Empty;
                    var isSwaggerUi = pathQuery == documentApiBaseUrl ||
                                      pathQuery == $"{documentApiBaseUrl}/index.html";
                    return isSwaggerUi;
                }

                private static bool IsSwaggerEndpoint(HttpContext httpContext)
                {
                    var pathQuery = httpContext.Request.Path.Value?.Trim('/').ToLower() ?? string.Empty;
                    var swaggerEndpoint = SwaggerEndpoint.Trim('/');
                    var isSwaggerEndPoint = pathQuery.StartsWith(swaggerEndpoint);
                    return isSwaggerEndPoint;
                }

                private static bool IsCanAccessSwagger(HttpContext httpContext)
                {
                    return DeveloperHelper.IsCanAccess(httpContext, ConfigurationRoot);
                }
            }
        }
    }
}