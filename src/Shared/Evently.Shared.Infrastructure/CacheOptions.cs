using Microsoft.Extensions.Caching.Distributed;

namespace Evently.Shared.Infrastructure;

public static class CacheOptions
{
    public static readonly DistributedCacheEntryOptions Default = new ()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3)
    };

    public static DistributedCacheEntryOptions Create(TimeSpan? expiration) => expiration is null
        ? Default
        : new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        };
}
