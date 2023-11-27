namespace Kotoba.Core.Exceptions;

public sealed class NoTranslationIntegrationConfiguredException : Exception
{
    internal NoTranslationIntegrationConfiguredException()
    {
    }

    internal NoTranslationIntegrationConfiguredException(string? message) : base(message)
    {
    }

    internal NoTranslationIntegrationConfiguredException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}