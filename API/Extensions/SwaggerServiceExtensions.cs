using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace API.Extensions
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

                // Configuring Authorization with Swagger - Accepting Bearer Token (i.e. in Authentication Header)
                // Alternative of this could be simply using Postman.
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
                {
                    Description = "Standard Authorization Header using the Bearer scheme {\"bearer {token}\"}",
                    In = ParameterLocation.Header, // The Locaton of the ApiKey. Valid values are "query", "header", and "cookie".
                    Name = "Authorization", // The name of the query, header or cookie parameter to be used.
                    Type = SecuritySchemeType.ApiKey, // The type of the security scheme. Valid values are "apiKey", "http", "oauth2", "openIdConnect"
                });
                c.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            return services;
        }
    }
}
