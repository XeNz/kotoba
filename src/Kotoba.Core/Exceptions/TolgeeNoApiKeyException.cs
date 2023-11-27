namespace Kotoba.Core.Exceptions;

public sealed class TolgeeNoApiKeyException : Exception
{
    public TolgeeNoApiKeyException()
    {
    }

    public TolgeeNoApiKeyException(string? message) : base(message)
    {
    }

    public TolgeeNoApiKeyException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}