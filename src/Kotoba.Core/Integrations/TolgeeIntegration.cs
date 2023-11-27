using System.IO.Compression;
using System.Text.Json;
using Kotoba.Core.Domain;
using Kotoba.Core.Utilities;
using Kotoba.Core.Exceptions;

namespace Kotoba.Core.Integrations;

internal sealed class TolgeeIntegration : ITranslationIntegration
{
    private readonly TolgeeConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public TolgeeIntegration(TolgeeConfiguration configuration, HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
    }

    public string Name => nameof(TolgeeIntegration);

    public async Task<IEnumerable<Translation>> FetchAsync()
    {
        var translations = new List<Translation>();

        var baseUrl = new Uri($"{_configuration.BaseUrl}/projects/export?ak={_configuration.ApiKey}&format=JSON&&zip=true&structureDelimiter=.");

        using var zipStream = await _httpClient.GetStreamAsync(baseUrl).ConfigureAwait(false);
        using var archive = new ZipArchive(zipStream);

        foreach (var entry in archive.Entries)
        {
            var newTranslations = await ProcessFile(entry).ConfigureAwait(false);
            translations.AddRange(newTranslations);
        }


        return translations;
    }

    private static async Task<IEnumerable<Translation>> ProcessFile(ZipArchiveEntry entry)
    {
        var translations = new List<Translation>();

        await using var entryStream = entry.Open().ConfigureAwait(false);

        var code = entry.Name;
        var languageCode = LanguageCodeHelpers.MatchLanguageCode(code);

        //TODO: there might still be some optimization to do, we're currently calling the sync version
        var jsonString = JsonSerializer.Serialize(entryStream);

        if (!jsonString.IsValidJson())
        {
            ThrowHelper.ThrowIsInvalidJsonException();
        }

        var flattenedToDotNotation = JsonUtilities.FlattenJsonToDotNotation(jsonString);

        foreach (var (key, value) in flattenedToDotNotation)
        {
            translations.Add(new Translation(languageCode, key, value));
        }

        return translations;
    }
}