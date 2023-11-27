using System.Text.Json;

namespace Kotoba.Core.Utilities;

internal static class JsonUtilities
{
    internal static bool IsValidJson(this string? source)
    {
        if (source == null)
        {
            return false;
        }

        try
        {
            JsonDocument.Parse(source);
            return true;
        }
        catch (JsonException)
        {
            return false;
        }
    }

    internal static Dictionary<string, string> FlattenJsonToDotNotation(
        string json,
        string parentKey = "",
        Dictionary<string, string>? result = null)
    {
        result ??= new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        using var document = JsonDocument.Parse(json);

        var root = document.RootElement;

        foreach (var property in root.EnumerateObject())
        {
            var key = string.IsNullOrEmpty(parentKey) ? property.Name : $"{parentKey}.{property.Name}";

            if (property.Value.ValueKind == JsonValueKind.Object)
            {
                FlattenJsonToDotNotation(property.Value.ToString(), key, result);
            }
            else
            {
                result[key] = property.Value.ToString();
            }
        }

        return result;
    }
}