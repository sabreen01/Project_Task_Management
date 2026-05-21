using System.Text.Json;
using Core.Application.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.Services;

public class CacheService(IDistributedCache distributedCache, ILogger<CacheService> logger) : ICacheService
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            var cachedData = await distributedCache.GetStringAsync(key, cancellationToken);
            if (string.IsNullOrEmpty(cachedData))
                return default;

            return JsonSerializer.Deserialize<T>(cachedData, _jsonOptions);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Error reading from cache for key: {CacheKey}", key);
            return default;
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expirationTime = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var serializedData = JsonSerializer.Serialize(value, _jsonOptions);
            
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expirationTime ?? TimeSpan.FromMinutes(10)
            };

            await distributedCache.SetStringAsync(key, serializedData, options, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Error writing to cache for key: {CacheKey}", key);
        }
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            await distributedCache.RemoveAsync(key, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Error removing from cache for key: {CacheKey}", key);
        }
    }
    
    
    public async Task InvalidateGroupAsync(string groupKey, CancellationToken cancellationToken = default)
    {
        try
        {
            var versionKey = $"{groupKey}_version";
            var currentVersion = await distributedCache.GetStringAsync(versionKey, cancellationToken);
            
            if (long.TryParse(currentVersion, out var version))
            {
                version++;
            }
            else
            {
                version = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            }
            
            await distributedCache.SetStringAsync(versionKey, version.ToString(), cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Error invalidating cache group: {GroupKey}", groupKey);
        }
    }

    public async Task<string> GetCacheKeyWithVersionAsync(string groupKey, string key, CancellationToken cancellationToken = default)
    {
        var versionKey = $"{groupKey}_version";
        var currentVersion = await distributedCache.GetStringAsync(versionKey, cancellationToken);
        
        if (string.IsNullOrEmpty(currentVersion))
        {
            currentVersion = "1";
            await distributedCache.SetStringAsync(versionKey, currentVersion, cancellationToken);
        }

        return $"{groupKey}_{key}_v{currentVersion}";
    }
}
