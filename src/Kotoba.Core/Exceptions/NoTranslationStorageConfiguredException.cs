namespace Kotoba.Core.Exceptions;

public sealed class NoTranslationStorageConfiguredException : Exception
{
    internal NoTranslationStorageConfiguredException()
    {
    }

    internal NoTranslationStorageConfiguredException(string? message) : base(message)
    {
    }

    internal NoTranslationStorageConfiguredException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}