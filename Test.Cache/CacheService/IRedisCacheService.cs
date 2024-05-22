using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Cache.CacheService
{
    public interface IRedisCacheService
    {
        Task<T?> GetValueAsync<T>(string key);
        
        Task SetValueAsync<T>(string key, T value);
        
        Task DeleteKeyAsync(string key);

        Task<bool> ExistsKeyAsync(string key);

        Task UpdateValueAsync<T>(string key, T value);
    }
}
