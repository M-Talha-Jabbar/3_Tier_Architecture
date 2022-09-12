using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Service.Contracts;
using Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extensions
{
    public static class RedisCacheServiceExtensions
    {
        public static IServiceCollection AddRedisCacheServices(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetSection("RedisCache:URL").Value;
            });

            services.AddScoped<ICacheService, CacheService>();

            return services;
        }
    }
}
