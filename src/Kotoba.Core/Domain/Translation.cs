namespace Kotoba.Core.Domain;

public readonly struct Translation : IEquatable<Translation>
{
    public Translation(LanguageCode languageCode, string key, string value)
    {
        LanguageCode = languageCode;
        Key = key;
        Value = value;
    }

    public LanguageCode LanguageCode { get; init; }

    /// <summary>
    /// The key used for the lookup
    /// </summary>
    public string Key { get; init; }

    /// <summary>
    /// The actual translation value
    /// </summary>
    public string Value { get; init; }

    public static bool operator ==(Translation left, Translation right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Translation left, Translation right)
    {
        return !(left == right);
    }

    public bool Equals(Translation other) =>
        LanguageCode == other.LanguageCode &&
        string.Equals(Key, other.Key, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) =>
        obj is Translation other && Equals(other);

    public override int GetHashCode() =>
        HashCode.Combine((int)LanguageCode, Key);
}