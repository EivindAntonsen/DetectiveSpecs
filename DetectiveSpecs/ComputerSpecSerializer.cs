using System.Text;
using DetectiveSpecs.Enums;

namespace DetectiveSpecs;

public class ComputerSpecSerializer
{
    private readonly StringBuilder stringBuilder = new();
    public readonly int PadLength = GetPadLength();



    public string Serialize(ComputerSpecs specs)
    {
        AppendSingleComponent(specs.Motherboard);
        AppendSingleComponent(specs.Cpu);
        AppendManyComponents(specs.Gpu);
        AppendManyComponents(specs.Memory);
        AppendManyComponents(specs.Storage);
        AppendManyComponents(specs.Network);
        AppendManyComponents(specs.Optical);
        AppendManyComponents(specs.Mouse);
        AppendManyComponents(specs.Keyboard);
        AppendManyComponents(specs.Sound);

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



    private void AppendSingleComponent(Component component)
    {
        stringBuilder.AppendLine(component.ComponentType.ToString());

        foreach (var (key, value) in component.Properties)
            stringBuilder.AppendLine($"  {key.ToString().PadRight(PadLength)}  {value}");
    }



    private void AppendManyComponents(IEnumerable<Component> components)
    {
        var array = components.ToArray();

        if (array.Length == 0)
            return;

        var index = 1;
        foreach (var component in array)
        {
            stringBuilder.AppendLine($"{component.ComponentType}" + (array.Length > 1 ? $"-{index}" : ""));
            index++;
            foreach (var (key, value) in component.Properties)
                stringBuilder.AppendLine($"  {key.ToString().PadRight(PadLength)}  {value}");
        }
    }
}