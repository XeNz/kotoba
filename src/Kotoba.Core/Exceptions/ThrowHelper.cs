using System.Diagnostics.CodeAnalysis;

namespace Kotoba.Core.Exceptions;

internal static class ThrowHelper
{
    [DoesNotReturn]
    internal static void ThrowNoTranslationIntegrationConfiguredException() =>
        throw new NoTranslationIntegrationConfiguredException("No translation integrations configured");

    [DoesNotReturn]
    internal static void ThrowNoTranslationStorageConfiguredException() =>
        throw new NoTranslationStorageConfiguredException("No translation storage configured");

    [DoesNotReturn]
    internal static void ThrowIsInvalidJsonException() =>
        throw new InvalidJsonException("Could not parse JSON");

    [DoesNotReturn]
    internal static void ThrowTolgeeNoApiKeyException() =>
        throw new TolgeeNoApiKeyException("No API key for Tolgee has been configured");
}