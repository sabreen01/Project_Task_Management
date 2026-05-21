namespace Core.Application.Interfaces;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);
    Task SetAsync<T>(string key, T value, TimeSpan? expirationTime = null, CancellationToken cancellationToken = default);
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);
    Task InvalidateGroupAsync(string groupKey, CancellationToken cancellationToken = default);
    Task<string> GetCacheKeyWithVersionAsync(string groupKey, string key, CancellationToken cancellationToken = default);
}
