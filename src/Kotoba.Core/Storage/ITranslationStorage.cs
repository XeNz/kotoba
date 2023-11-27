using Kotoba.Core.Domain;

namespace Kotoba.Core.Storage;

public interface ITranslationStorage
{
    /// <summary>
    /// Name of the storage implementation
    /// </summary>
    string Name { get; }

    ValueTask<Translation?> GetTranslationAsync(LanguageCode languageCode, string key);

    Task FillAsync(IEnumerable<Translation> translations);
}