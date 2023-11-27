namespace Kotoba.Core.Exceptions;

public sealed class InvalidJsonException : Exception
{
    internal InvalidJsonException()
    {
    }

    internal InvalidJsonException(string? message) : base(message)
    {
    }

    internal InvalidJsonException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}