using StackExchange.Redis;
using Newtonsoft.Json;

namespace Test.Cache.CacheService
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        public RedisCacheService(IConnectionMultiplexer conenctionMultiplexer) => _connectionMultiplexer = conenctionMultiplexer;

        public async Task DeleteKeyAsync(string key)
        {
            var db = _connectionMultiplexer.GetDatabase();
            await db.KeyDeleteAsync(key);
        }

        public async Task<bool> ExistsKeyAsync(string key)
        {
            var db = _connectionMultiplexer.GetDatabase();
            return await db.KeyExistsAsync(key);
        }

        public async Task<T?> GetValueAsync<T>(string key)
        {
            var db = _connectionMultiplexer.GetDatabase();
            var cacheObject = await db.StringGetAsync(key);

            return cacheObject == RedisValue.Null ? default : JsonConvert.DeserializeObject<T>(cacheObject);
        }

        public async Task SetValueAsync<T>(string key, T value)
        {
            var db = _connectionMultiplexer.GetDatabase();
            var jsonValue = JsonConvert.SerializeObject(value);
            await db.StringSetAsync(key, jsonValue);
        }

        public async Task UpdateValueAsync<T>(string key, T value)
        {
            await SetValueAsync<T>(key, value);
        }
    }
}
