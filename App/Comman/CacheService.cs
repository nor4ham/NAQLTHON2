using Comman.InterfaceCacheService;
using Microsoft.EntityFrameworkCore.Storage;
namespace Comman.CacheService;

using System.Text.Json;
using StackExchange.Redis;
 public class CacheService : ICacheService
    {
        private IDatabase _cacheDb;
          ILogger<CacheService> _logger;
        public CacheService(ILogger<CacheService> logger, IConfiguration configuration)
        {
             _logger = logger;
             var redisConnectionString = configuration.GetConnectionString("RedisConnection");
            var redis = ConnectionMultiplexer.Connect(redisConnectionString);
            _cacheDb = redis.GetDatabase();

        }
        
public T GetData<T>(string key)
{
    var value = _cacheDb.StringGet(key);
    if (!string.IsNullOrEmpty(value))
    {
        try
        {
            return JsonSerializer.Deserialize<T>(value);
        }
        catch (JsonException ex)
        {
            // Log the exception details
            // You can use any logging framework that you prefer
            // For example, using Microsoft.Extensions.Logging:
            _logger.LogError(ex, "Json deserialization failed for key {Key} with value {Value}", key, value);

            // Optionally, you can return a default value
            // return default;

            // Or, if the situation warrants it, you can throw a custom exception
            // This could be a custom exception class that your application defines
            // throw new MyCustomException("Deserialization failed", ex);

            // For now, let's just rethrow the original exception
            // This will propagate the error to the caller, which might be appropriate
            // depending on how you want to handle these cases
            throw;
        }
    }
    return default;
}

public object RemoveData(string key)
{
    bool _isKeyExist = _cacheDb.KeyExists(key);
    if (_isKeyExist == true)
    {
        return _cacheDb.KeyDelete(key);
    }
    return false;
}

    public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
    {
       TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
        var isSet =_cacheDb.StringSet(key, JsonSerializer.Serialize(value), expiryTime);
        return isSet;
    }

}
