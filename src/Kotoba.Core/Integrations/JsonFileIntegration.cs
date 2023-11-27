using Kotoba.Core.Domain;
using Kotoba.Core.Exceptions;
using Kotoba.Core.Utilities;

namespace Kotoba.Core.Integrations;

internal sealed class JsonFileIntegration : ITranslationIntegration
{
    private readonly JsonFileConfiguration _jsonFileConfiguration;

    public JsonFileIntegration(JsonFileConfiguration jsonFileConfiguration)
    {
        _jsonFileConfiguration = jsonFileConfiguration;
    }

    public string Name => nameof(JsonFileIntegration);

    public async Task<IEnumerable<Translation>> FetchAsync()
    {
        var translations = new List<Translation>();

        foreach (var file in _jsonFileConfiguration.Files)
        {
            await ProcessFileAsync(file, translations).ConfigureAwait(false);
        }

        return translations;
    }

    private static async Task ProcessFileAsync(JsonFileInfo file, ICollection<Translation> translations)
    {
        var languageCode = LanguageCodeHelpers.MatchLanguageCode(file.LanguageCode);

        using var reader = new StreamReader(file.Path);
        var jsonString = await reader.ReadToEndAsync().ConfigureAwait(false);

        if (!jsonString.IsValidJson())
        {
            ThrowHelper.ThrowIsInvalidJsonException();
        }

        var flattenedToDotNotation = JsonUtilities.FlattenJsonToDotNotation(jsonString);

        foreach (var (key, value) in flattenedToDotNotation)
        {
            translations.Add(new Translation(languageCode, key, value));
        }
    }
}