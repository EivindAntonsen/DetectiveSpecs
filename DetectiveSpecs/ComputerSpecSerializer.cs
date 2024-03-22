using System.Text;
using DetectiveSpecs.Enums;

namespace DetectiveSpecs;

public class ComputerSpecSerializer
{
    private readonly StringBuilder stringBuilder = new();
    public readonly int PadLength = GetPadLength();



    public string Serialize(ComputerSpecs specs)
    {
        foreach (Component component in specs.GetAllComponents) 
            AppendComponent(component);
        
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



    private void AppendComponent(Component component)
    {
        stringBuilder.AppendLine(component.ComponentType.ToString());

        foreach (var (key, value) in component.Properties)
            stringBuilder.AppendLine($"  {key.ToString().PadRight(PadLength)}  {value}");
    }
}