using System.Text;
using DetectiveSpecs.Models;

namespace DetectiveSpecs;


public static class ComputerSpecSerializer
{
    public static string Serialize(ComputerSpecs specs, string path)
    {
        var stringBuilder = new StringBuilder();
        var padToLength = Enum
            .GetValues<ComponentProperty>()
            .Select(componentProperty => componentProperty.ToString())
            .Max(s => s.Length);

        AppendSingleComponent(ComponentType.Motherboard, specs.Motherboard, stringBuilder, padToLength);
        AppendSingleComponent(ComponentType.Cpu, specs.Cpu, stringBuilder, padToLength);
        AppendManyComponents(ComponentType.Gpu, specs.Gpu, stringBuilder, padToLength);
        AppendManyComponents(ComponentType.Memory, specs.Memory, stringBuilder, padToLength);
        AppendManyComponents(ComponentType.Storage, specs.Storage, stringBuilder, padToLength);
        AppendManyComponents(ComponentType.Network, specs.Network, stringBuilder, padToLength);
        AppendManyComponents(ComponentType.Optical, specs.Optical, stringBuilder, padToLength);
        AppendManyComponents(ComponentType.Mouse, specs.Mouse, stringBuilder, padToLength);
        AppendManyComponents(ComponentType.Keyboard, specs.Keyboard, stringBuilder, padToLength);
        AppendManyComponents(ComponentType.Sound, specs.Sound, stringBuilder, padToLength);

        return stringBuilder.ToString();
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