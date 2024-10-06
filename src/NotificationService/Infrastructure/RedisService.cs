using System.Text.Json;
using StackExchange.Redis;

namespace NotificationService.Infrastructure;

public class RedisService(IConnectionMultiplexer redis) : IRedisService
{
    private readonly IDatabase _database = redis.GetDatabase();

    public async Task<bool> DeleteAsync(string key)
    {
        return await _database.KeyDeleteAsync(key);
    }

    public async Task<T> GetValueAsync<T>(string key)
    {
        var data = await _database.StringGetAsync(key);

        return data.IsNullOrEmpty ? default : JsonSerializer.Deserialize<T>(data);
    }

    public async Task<T> UpdateValueAsync<T>(string key, T value)
    {
        var created = await _database.StringSetAsync(key,
            JsonSerializer.Serialize(value), TimeSpan.FromDays(10));

        if (!created) return default;

        return await GetValueAsync<T>(key);
    }
}