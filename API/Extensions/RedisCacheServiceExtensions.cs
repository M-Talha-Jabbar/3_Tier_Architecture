using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = Configuration.GetSection("RedisCache:URL").Value;
            });

            return services;
        }
    }
}
