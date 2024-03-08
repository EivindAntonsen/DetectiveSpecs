using System.Text;
using DetectiveSpecs.Enums;

namespace DetectiveSpecs;

public class ComputerSpecSerializer
{
    private readonly StringBuilder stringBuilder = new();
    private readonly int padLength = GetPadLength();



    public string Serialize(ComputerSpecs specs)
    {
        AppendSingleComponent(ComponentType.Motherboard, specs.Motherboard);
        AppendSingleComponent(ComponentType.Cpu, specs.Cpu);
        AppendManyComponents(ComponentType.Gpu, specs.Gpu);
        AppendManyComponents(ComponentType.Memory, specs.Memory);
        AppendManyComponents(ComponentType.Storage, specs.Storage);
        AppendManyComponents(ComponentType.Network, specs.Network);
        AppendManyComponents(ComponentType.Optical, specs.Optical);
        AppendManyComponents(ComponentType.Mouse, specs.Mouse);
        AppendManyComponents(ComponentType.Keyboard, specs.Keyboard);
        AppendManyComponents(ComponentType.Sound, specs.Sound);

        return stringBuilder.ToString();
    }



    /// <summary>
    /// To ensure a visually sensible output, we append a minimum length to the keys
    /// so that the values will line up as if on a column.
    /// </summary>
    /// <returns></returns>
    private static int GetPadLength() => Enum
        .GetValues<ComponentProperty>()
        .Select(componentProperty => componentProperty.ToString())
        .Max(s => s.Length);



    private void AppendSingleComponent(ComponentType componentType, Component component)
    {
        stringBuilder.AppendLine(componentType.ToString());

        foreach (var (key, value) in component.Properties)
            stringBuilder.AppendLine($"  {key.ToString().PadRight(padLength)}  {value}");
    }



    private void AppendManyComponents(ComponentType componentType, IEnumerable<Component> components)
    {
        var array = components.ToArray();

        if (array.Length == 0)
            return;

        var index = 1;
        foreach (var component in array)
        {
            stringBuilder.AppendLine($"{componentType}" + (array.Length > 1 ? $"-{index}" : ""));
            index++;
            foreach (var (key, value) in component.Properties)
                stringBuilder.AppendLine($"  {key.ToString().PadRight(padLength)}  {value}");
        }
    }
}