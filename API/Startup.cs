using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repository.Contracts;
using Repository.Data;
using Repository.Repositories;
using Service.Contracts;
using Service.Services;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SchoolDBContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("SchoolDB"))
            );

            services.AddControllers(); 

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IStudentRepository, StudentRepository>();

            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();


            // validating token
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters()
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("JWT:SecretKey").Value)),
                            // We have given the Key for validating token but how does he know that we have used HmacSha512 algorithm for hashing?
                            // And that is from the Header part of this Bearer Token. The Header part consist of a property named 'alg' that tells which hashing algorithm has been applied on this token.

                            ValidateIssuer = false,
                            ValidateAudience = false
                        };
                    });
            // If your token is not valid, you will get an http response 401 (unauthorized).
            // If your token is valid, but your role claim (i.e. in payload) doesn't allow you to access a particular route/resource then you will get an http response 403 (forbidden) that means insufficient rights to a resource.

            // What if token is expired?
            // Is Refresh token will remain safe beacuse with it we will another jwt?

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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication(); // enables authentication capabilities

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
