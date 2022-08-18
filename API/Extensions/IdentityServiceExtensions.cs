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
                    // JWT Authentication Handler for authenticating subsequent request (validating JWT before giving access to a route/resource)
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

            // If your JWT is valid, but your role claim (i.e. in payload) doesn't allow you to access a particular route/resource then you will get an http response 403 (forbidden) (by UseAuthorization() middleware) that means insufficient rights to a resource.


            services.AddAuthorization(configure =>
            {
                configure.AddPolicy("Trusted", policy =>
                {
                    policy.RequireAuthenticatedUser(); // similar to adding [Authorize] attribute without specifiying any Policy and Roles on controller or action method.
                    // [Authorize] attribute without any Policy & Roles specified, just authenticate the subsequent request means whether the User have valid JWT or valid cookie based upon how authentication has been configured.

                    policy.RequireAssertion(context => // RequireAssertion() is used to define Custom Policy Requirement.
                    {
                        return (context.User.IsInRole("HR") && context.User.HasClaim("Rights", "Create")) || context.User.IsInRole("Manager Academics");
                    }); // The custom lambda function must return a true if the policy requirement is satisfied.
                    //policy.RequireRole("HR")
                    //policy.RequireClaim("Rights", "Create"); // ClaimType comparison is case in-sensitive where as ClaimValue comparison is case sensitive.
                });
            });

            // Policy defines a collection of requirements, that the user must satisfy in order to access a resource.
            // We make policy when we want to filter out the people of a particular role on the basis of their individual claims (not on role claims). OR
            // Filtering out the people with different roles having overlapping individual claims (i.e. same clamis/rights).

            return services;
        }
    }
}
