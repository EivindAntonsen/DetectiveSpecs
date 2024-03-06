using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using DetectiveSpecs.Models;

namespace DetectiveSpecs;

[SuppressMessage("Performance", "CA1869:Cache and reuse \'JsonSerializerOptions\' instances")]
public static class FileWriter
{
    public static async Task<string> WriteAsJson(ComputerSpecs specs, string path)
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



    public static async Task<string> WriteAsCustomFormat(ComputerSpecs specs, string path)
    {
        var sb = new StringBuilder();
        var pad = GetLongestPropertyName(specs);

        AppendSingleComponent(ComponentType.Motherboard, specs.Motherboard, sb, pad);
        AppendSingleComponent(ComponentType.Cpu, specs.Cpu, sb, pad);
        AppendComponents(ComponentType.Gpu, specs.Gpu, sb, pad);
        AppendComponents(ComponentType.Memory, specs.Memory, sb, pad);
        AppendComponents(ComponentType.Storage, specs.Storage, sb, pad);
        AppendComponents(ComponentType.Network, specs.Network, sb, pad);
        AppendComponents(ComponentType.Optical, specs.Optical, sb, pad);
        AppendComponents(ComponentType.Mouse, specs.Mouse, sb, pad);
        AppendComponents(ComponentType.Keyboard, specs.Keyboard, sb, pad);
        AppendComponents(ComponentType.Sound, specs.Sound, sb, pad);

        var filename = $"{path}.txt";

        await File.WriteAllTextAsync(filename, sb.ToString()).ConfigureAwait(false);

        return filename;
    }



    private static int GetLongestPropertyName(ComputerSpecs specs)
    {
        var maxCpu = GetMaxPropertyNameLengthOfOne(specs.Cpu);
        var maxMobo = GetMaxPropertyNameLengthOfOne(specs.Motherboard);
        var maxGpu = GetMaxPropertyNameLengthOfMany(specs.Gpu);
        var maxMemory = GetMaxPropertyNameLengthOfMany(specs.Memory);
        var maxStorage = GetMaxPropertyNameLengthOfMany(specs.Storage);
        var maxNetwork = GetMaxPropertyNameLengthOfMany(specs.Network);
        var maxOptical = GetMaxPropertyNameLengthOfMany(specs.Optical);
        var maxMouse = GetMaxPropertyNameLengthOfMany(specs.Mouse);
        var maxKeyboard = GetMaxPropertyNameLengthOfMany(specs.Keyboard);
        var maxSound = GetMaxPropertyNameLengthOfMany(specs.Sound);
        
        return new[] { maxCpu, maxMobo, maxGpu, maxMemory, maxStorage, maxNetwork, maxOptical, maxMouse, maxKeyboard, maxSound }.Max();

        int GetMaxPropertyNameLengthOfMany(IEnumerable<Component> components)
        {
            var array = components.ToArray();
            if (array.Length == 0) return default;
            
            return array
                .SelectMany(kvp => kvp.Properties)
                .Max(kvp => kvp.Key.ToString().Length);
        }

        int GetMaxPropertyNameLengthOfOne(Component component) => component.Properties
            .Max(kvp => kvp.Key.ToString().Length);
    }



    private static void AppendSingleComponent(ComponentType componentType, Component component, StringBuilder sb, int pad)
    {
        sb.AppendLine(componentType.ToString());
        
        foreach (var (key, value) in component.Properties)
            sb.AppendLine($"  {key.ToString().PadRight(pad)}\t{value}");
    }



    private static void AppendComponents(ComponentType componentType, IEnumerable<Component> components, StringBuilder sb, int pad)
    {
        var index = 1;
        var array = components.ToArray();

        if (array.Length == 0)
            return;

        foreach (var component in array)
        {
            sb.AppendLine($"{componentType}-{index}");
            index++;
            foreach (var (key, value) in component.Properties)
                sb.AppendLine($"  {key.ToString().PadRight(pad)}\t{value}");
        }
    }
}