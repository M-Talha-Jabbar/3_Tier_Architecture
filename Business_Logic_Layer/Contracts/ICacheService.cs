using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface ICacheService
    {
        Task<T> GetData<T>(string key);
        Task SetData<T>(string key, T value, TimeSpan slidingExpiration, TimeSpan absoluteExpiration);
        Task RemoveData(string key);
        Task RefreshSlidingExpirationTime(string key);
    }
}
