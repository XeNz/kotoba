using Kotoba.Core.Exceptions;

namespace Kotoba.Core.Integrations;

#pragma warning disable CA1056
#pragma warning disable CA1054
public record TolgeeConfiguration(string ApiKey, string BaseUrl = "https://app.tolgee.io/v2") : IValidatableIntegrationConfiguration
#pragma warning restore CA1054
#pragma warning restore CA1056
{
    public void Validate()
    {
        if (string.IsNullOrEmpty(ApiKey))
        {
            ThrowHelper.ThrowTolgeeNoApiKeyException();
        }
    }
}