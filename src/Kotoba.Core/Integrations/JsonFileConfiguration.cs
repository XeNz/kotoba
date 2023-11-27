namespace Kotoba.Core.Integrations;

public record JsonFileConfiguration(IEnumerable<JsonFileInfo> Files) : IValidatableIntegrationConfiguration
{
    public void Validate()
    {
    }
}