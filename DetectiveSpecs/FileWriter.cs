using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using YamlDotNet.Serialization;

namespace DetectiveSpecs;

[SuppressMessage("Performance", "CA1869:Cache and reuse \'JsonSerializerOptions\' instances")]
public static class FileWriter
{
    public static async Task<string> WriteAsJson(object specs, string path)
    {
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };
        var serialize = JsonSerializer.Serialize(specs, jsonSerializerOptions);
        var filename = $"{path}.json";

        await File.WriteAllTextAsync(filename, serialize).ConfigureAwait(false);

        return filename;
    }



    public static async Task<string> WriteAsYaml(object obj, string path)
    {
        var yaml = new SerializerBuilder().Build().Serialize(obj);
        var filename = $"{path}.yaml";

        await File.WriteAllTextAsync($"{path}.yaml", yaml).ConfigureAwait(false);

        return filename;
    }
}