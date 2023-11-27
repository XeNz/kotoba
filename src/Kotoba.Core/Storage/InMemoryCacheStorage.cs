using Kotoba.Core.Constants;
using Kotoba.Core.Domain;
using Microsoft.Extensions.Caching.Memory;

namespace Kotoba.Core.Storage;

internal sealed class InMemoryCacheStorage : ITranslationStorage
{
    private readonly IMemoryCache _memoryCache;

    public InMemoryCacheStorage(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public string Name => nameof(InMemoryCacheStorage);

    // 1 lookup only instead of double lookup when grouped by language
    public ValueTask<Translation?> GetTranslationAsync(LanguageCode languageCode, string key)
    {
        var cacheKey = $"{CacheKeyConstants.CacheKeyPrefix}_{languageCode.ToStringFast()}_{key}";
        _memoryCache.TryGetValue(cacheKey, out string? translationValue);

        return translationValue is null
            ? ValueTask.FromResult<Translation?>(null)
            : ValueTask.FromResult<Translation?>(new Translation(languageCode, key, translationValue));
    }

    // TODO: add some kind of logging, preferable with source generator, that shows how many records have been persisted into cache 
    public Task FillAsync(IEnumerable<Translation> translations)
    {
        foreach (var translation in translations)
        {
            var cacheKey = $"{CacheKeyConstants.CacheKeyPrefix}_{translation.LanguageCode.ToStringFast()}_{translation.Key}";

            using var entry = _memoryCache.CreateEntry(cacheKey);
            entry.Value = translation.Value;
        }

        return Task.CompletedTask;
    }
}