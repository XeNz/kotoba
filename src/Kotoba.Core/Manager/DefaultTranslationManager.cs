using Kotoba.Core.Domain;
using Kotoba.Core.Exceptions;
using Kotoba.Core.Integrations;
using Kotoba.Core.Storage;
using Microsoft.Extensions.Logging;

namespace Kotoba.Core.Manager;

internal sealed class DefaultTranslationManager : ITranslationManager
{
    private readonly ILogger<DefaultTranslationManager> _logger;

    private readonly IEnumerable<ITranslationStorage> _translationStorages;
    private readonly IEnumerable<ITranslationIntegration> _translationProviders;

    public DefaultTranslationManager(
        IEnumerable<ITranslationStorage> translationStorages,
        IEnumerable<ITranslationIntegration> translationProviders,
        ILogger<DefaultTranslationManager> logger)
    {
        _translationStorages = translationStorages;
        _translationProviders = translationProviders;
        _logger = logger;
    }

    public async Task FetchAndPersistAsync()
    {
        Validate(_translationProviders, _translationStorages);

        var translations = new List<Translation>();

        foreach (var translationProvider in _translationProviders)
        {
            await FetchAsync(translationProvider, translations).ConfigureAwait(false);
        }

        foreach (var translationStorage in _translationStorages)
        {
            await PersistAsync(translationStorage, translations).ConfigureAwait(false);
        }
    }

    private async Task FetchAsync(ITranslationIntegration translationProvider, List<Translation> translations)
    {
        try
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Fetching translations from {Provider}", translationProvider.Name);
            }

            var fetched = await translationProvider.FetchAsync().ConfigureAwait(false);
            translations.AddRange(fetched);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to fetch translations from {Provider}", translationProvider.Name);
            throw;
        }
    }

    private async Task PersistAsync(ITranslationStorage translationStorage, List<Translation> translations)
    {
        try
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Persisting translations in {Storage}", translationStorage.Name);
            }

            await translationStorage.FillAsync(translations).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to persist translations in {Storage}", translationStorage.Name);
            throw;
        }
    }

    private static void Validate(IEnumerable<ITranslationIntegration> translationIntegrations, IEnumerable<ITranslationStorage> translationStorages)
    {
        if (!translationIntegrations.Any())
        {
            ThrowHelper.ThrowNoTranslationIntegrationConfiguredException();
        }

        if (!translationStorages.Any())
        {
            ThrowHelper.ThrowNoTranslationStorageConfiguredException();
        }
    }
}