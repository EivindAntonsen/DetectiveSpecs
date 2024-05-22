using System.Text;
using DetectiveSpecs.Enums;

namespace DetectiveSpecs;

public class HardwareInfoSerializer
{
    private readonly StringBuilder _stringBuilder = new();
    public readonly int PadLength = GetPadLength();



    public string Serialize(HardwareInfo hardwareInfo)
    {
        foreach (var component in hardwareInfo.GetAllComponents)
            AppendComponent(component);

        return _stringBuilder.ToString();
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
        _stringBuilder.AppendLine(component.ComponentType.ToString());

        foreach (var (key, value) in component.Properties)
            _stringBuilder.AppendLine($"  {key.ToString().PadRight(PadLength)}  {value}");
    }
}