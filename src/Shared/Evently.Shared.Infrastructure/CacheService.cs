using System.Buffers;
using System.Text.Json;
using Evently.Shared.Application.Cache;
using Microsoft.Extensions.Caching.Distributed;

namespace Evently.Shared.Infrastructure;

internal sealed class CacheService(IDistributedCache cache) : ICacheService
{
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        byte[]? bytes = await cache.GetAsync(key, cancellationToken);
        if (bytes is null)
        {
            return default;
        }
        return JsonSerializer.Deserialize<T>(bytes);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        var buffer = new ArrayBufferWriter<byte>();
        await using var writer = new Utf8JsonWriter(buffer);
        JsonSerializer.Serialize(writer, value);
        
        await cache.SetAsync(key, buffer.WrittenSpan.ToArray(), CacheOptions.Create(expiration), cancellationToken);
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default) =>
        cache.RemoveAsync(key, cancellationToken);
}
