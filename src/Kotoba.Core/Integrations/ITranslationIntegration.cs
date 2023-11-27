using Kotoba.Core.Domain;

namespace Kotoba.Core.Integrations;

public interface ITranslationIntegration
{
    /// <summary>
    /// Name of the translation provider implementation
    /// </summary>
    string Name { get; }

    Task<IEnumerable<Translation>> FetchAsync();
}