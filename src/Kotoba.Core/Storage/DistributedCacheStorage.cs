using Kotoba.Core.Constants;
using Kotoba.Core.Domain;
using Kotoba.Core.Extensions;
using Microsoft.Extensions.Caching.Distributed;

namespace Kotoba.Core.Storage;

internal sealed class DistributedCacheStorage : ITranslationStorage
{
    private readonly IDistributedCache _distributedCache;

    public DistributedCacheStorage(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public string Name => nameof(DistributedCacheStorage);

    public async ValueTask<Translation?> GetTranslationAsync(LanguageCode languageCode, string key)
    {
        var cacheKey = $"{CacheKeyConstants.CacheKeyPrefix}_{languageCode.ToStringFast()}_{key}";
        var translationValue = await _distributedCache.GetStringAsync(cacheKey).ConfigureAwait(false);

        return translationValue is null ? null : new Translation(languageCode, key, translationValue);
    }

    // TODO: I don't like that this creates a new func per item
    // would rather like to see a cached delegate that allows me to pass in the _distributedCache, and allows me to make the lambda static
    // TODO: add some kind of logging, preferable with source generator, that shows how many records have been persisted into cache 
    public async Task FillAsync(IEnumerable<Translation> translations)
    {
        await translations.WhenAllPartitioned(5,
            async translation =>
            {
                var cacheKey = $"{CacheKeyConstants.CacheKeyPrefix}_{translation.LanguageCode.ToStringFast()}_{translation.Key}";
                await _distributedCache.SetStringAsync(cacheKey, translation.Value).ConfigureAwait(false);
            }).ConfigureAwait(false);
    }
}