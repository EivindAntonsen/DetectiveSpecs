using System.Diagnostics.CodeAnalysis;
using System.Text;
using DetectiveSpecs.Models;

namespace DetectiveSpecs;

[SuppressMessage("Performance", "CA1869:Cache and reuse \'JsonSerializerOptions\' instances")]
public static class ComputerSpecSerializer
{
    public static string Serialize(ComputerSpecs specs, string path)
    {
        var sb = new StringBuilder();
        var pad = Enum.GetValues<ComponentProperty>()
            .Select(componentProperty => componentProperty.ToString().Length)
            .Max();

        AppendSingleComponent(ComponentType.Motherboard, specs.Motherboard, sb, pad);
        AppendSingleComponent(ComponentType.Cpu, specs.Cpu, sb, pad);
        AppendManyComponents(ComponentType.Gpu, specs.Gpu, sb, pad);
        AppendManyComponents(ComponentType.Memory, specs.Memory, sb, pad);
        AppendManyComponents(ComponentType.Storage, specs.Storage, sb, pad);
        AppendManyComponents(ComponentType.Network, specs.Network, sb, pad);
        AppendManyComponents(ComponentType.Optical, specs.Optical, sb, pad);
        AppendManyComponents(ComponentType.Mouse, specs.Mouse, sb, pad);
        AppendManyComponents(ComponentType.Keyboard, specs.Keyboard, sb, pad);
        AppendManyComponents(ComponentType.Sound, specs.Sound, sb, pad);

        return sb.ToString();
    }



    private static void AppendSingleComponent(ComponentType componentType, Component component, StringBuilder sb, int pad)
    {
        sb.AppendLine(componentType.ToString());

        foreach (var (key, value) in component.Properties)
            sb.AppendLine($"  {key.ToString().PadRight(pad)}  {value}");
    }



    private static void AppendManyComponents(ComponentType componentType, IEnumerable<Component> components, StringBuilder sb, int pad)
    {
        var array = components.ToArray();

        if (array.Length == 0)
            return;

        var index = 1;
        foreach (var component in array)
        {
            sb.AppendLine($"{componentType}" + (array.Length > 1 ? $"-{index}" : ""));
            index++;
            foreach (var (key, value) in component.Properties)
                sb.AppendLine($"  {key.ToString().PadRight(pad)}  {value}");
        }
    }
}