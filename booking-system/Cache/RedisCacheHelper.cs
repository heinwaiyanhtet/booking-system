using StackExchange.Redis;
using Microsoft.Extensions.Configuration;

namespace BookingSystem.Cache
{
    public class RedisCacheHelper
    {
        private readonly IDatabase _db;
        public RedisCacheHelper(IConfiguration configuration)
        {
            var conn = configuration.GetConnectionString("Redis") ?? "localhost:6379";
            var mux = ConnectionMultiplexer.Connect(conn);
            _db = mux.GetDatabase();
        }

        public async Task<bool> AcquireLockAsync(string key, string value, TimeSpan expiry)
        {
            return await _db.StringSetAsync(key, value, expiry, When.NotExists);
        }

        public async Task ReleaseLockAsync(string key, string value)
        {
            var current = await _db.StringGetAsync(key);
            if (current == value)
            {
                await _db.KeyDeleteAsync(key);
            }
        }

         public async Task<long> IncrementAsync(string key)
        {
            return await _db.StringIncrementAsync(key);
        }

        public async Task<long> DecrementAsync(string key)
        {
            return await _db.StringDecrementAsync(key);
        }

        public async Task<long?> GetIntAsync(string key)
        {
            var value = await _db.StringGetAsync(key);
            if (value.IsNull)
            {
                return null;
            }
            if (long.TryParse(value!, out var result))
            {
                return result;
            }
            return null;
        }

        public async Task SetIntAsync(string key, long value)
        {
            await _db.StringSetAsync(key, value);
        }
    }
}