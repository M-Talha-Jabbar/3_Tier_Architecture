using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    // configuring authorization/validation middleware for validating JWT before giving access to a route/resource
                    .AddJwtBearer(options =>
                    {
                        // here we have specified which parameters must be taken into account to consider JWT as valid
                        options.TokenValidationParameters = new TokenValidationParameters()
                        {
                            // validates the signature of the JWT
                            // If your JWT is not valid, you will get an http response 401 (unauthorized).
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("JWT:SecretKey").Value)),
                            // We have given the Key for validating JWT but how does he know that we have used HmacSha512 algorithm for hashing?
                            // And that is from the Header part of this JWT. The Header part consist of a property named 'alg' that tells which hashing algorithm has been applied on this JWT.

                            ValidateIssuer = false,
                            ValidateAudience = false,

                            // validates that the JWT is not expired 
                            // If your JWT is not valid or it is expired, you will get an http response 401 (unauthorized).
                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.Zero // The default value of ClockSkew is 5 minutes. That means if you haven't set it, your JWT will be valid for Expiry Time + 5 mins.
                            // If you want to expire your JWT on the exact time (i.e. Expiry Time) you will need to set ClockSkew to zero.
                        };
                    });
            // If your JWT is valid, but your role claim (i.e. in payload) doesn't allow you to access a particular route/resource then you will get an http response 403 (forbidden) that means insufficient rights to a resource.

            return services;
        }
    }
}
