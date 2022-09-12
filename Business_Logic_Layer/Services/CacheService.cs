using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services // Cache-Aside Pattern (one of the Caching Patterns/Policies) is used.
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;

        public CacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<T> GetData<T>(string key)
        {
            var DeserializedData = await _distributedCache.GetStringAsync(key);

            if(DeserializedData != null) // if it is true than that means key is present in our Redis Cache.
            {
                var data = JsonConvert.DeserializeObject<T>(DeserializedData);

                return data;
            }

            return default(T); // default() produces the default value of a type. Here it will produce null.
        }

        public async Task RemoveData(string key)
        {
            await _distributedCache.RemoveAsync(key);
        }

        public async Task SetData<T>(string key, T value, TimeSpan slidingExpiration, TimeSpan absoluteExpiration)
        {
            var SerializedData = JsonConvert.SerializeObject(value);

            var option = new DistributedCacheEntryOptions()
                                        .SetSlidingExpiration(slidingExpiration) // Cached object expires if it not being requested for a defined amount of time period.
                                        .SetAbsoluteExpiration(absoluteExpiration); // Expiration time of the cached object.
                                                                                    // Note that Sliding Expiration should always be set lower than the Absolute Expiration.

            await _distributedCache.SetStringAsync(key, SerializedData, option);

            // Since we have stored the data in serialized form, so to get access to this data from redis-cli use (DUMP <Key>) command.
        }

        public async Task RefreshSlidingExpirationTime(string key)
        {
            await _distributedCache.RefreshAsync(key); // Reset Sliding Expiration Time if Key is present in Redis Cache
        }
    }
}
