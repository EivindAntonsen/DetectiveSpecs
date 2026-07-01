using System.Text;
using DetectiveSpecs.Enums;

namespace DetectiveSpecs;

public class HardwareInfoSerializer
{
    private readonly StringBuilder _stringBuilder = new();
    public int PadLength { get; } = GetPadLength();

    public string Serialize(HardwareInfo hardwareInfo)
    {
        _stringBuilder.Clear();
        foreach (var component in hardwareInfo.GetAllComponents)
            AppendComponent(component);

        return _stringBuilder.ToString();
    }

    private static int GetPadLength() => 
        Enum.GetValues<ComponentProperty>()
            .Max(p => p.ToString().Length);

    private void AppendComponent(Component component)
    {
        _stringBuilder.AppendLine(component.ComponentType.ToString());

        foreach (var (key, value) in component.Properties.OrderBy(kv => kv.Key))
            _stringBuilder.AppendLine($"  {key.ToString().PadRight(PadLength)}  {value}");
    }
}