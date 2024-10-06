namespace NotificationService.Infrastructure;

public interface IRedisService
{
    Task<T> GetValueAsync<T>(string key);
    Task<T> UpdateValueAsync<T>(string key, T value);
    Task<bool> DeleteAsync(string key);
}