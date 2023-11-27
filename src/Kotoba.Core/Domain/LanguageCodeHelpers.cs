namespace Kotoba.Core.Domain;

internal static class LanguageCodeHelpers
{
    internal readonly static LanguageCode[] Members = LanguageCodeExtensions.GetValues();

    internal static LanguageCode MatchLanguageCode(string code)
    {
        // TODO what to do when we cannot match? Right now it will take the default enum value
        var languageCode = Members.FirstOrDefault(languageCode =>
            string.Equals(languageCode.ToStringFast(),
                code,
                StringComparison.OrdinalIgnoreCase
            )
        );
        return languageCode;
    }
}