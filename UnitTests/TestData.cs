using DetectiveSpecs;
using DetectiveSpecs.Enums;

namespace UnitTests;

public static class TestData
{
    /// <summary>
    /// Creates an instance of ComputerSpecs with default values.
    /// </summary>
    /// <returns>A new instance of the ComputerSpecs class.</returns>
    public static HardwareInfo CreateComputerSpecs() => new(
        new List<Component>
        {
            CreateComponent(ComponentType.Cpu),
            CreateComponent(ComponentType.Gpu),
            CreateComponent(ComponentType.Motherboard),
            CreateComponent(ComponentType.Storage),
            CreateComponent(ComponentType.Memory),
            CreateComponent(ComponentType.Optical),
            CreateComponent(ComponentType.Network),
            CreateComponent(ComponentType.Sound),
            CreateComponent(ComponentType.Keyboard),
            CreateComponent(ComponentType.Mouse)
        }
    );



    private static Component CreateComponent(ComponentType componentType) =>
        new(componentType, GetComponentProperties(componentType));



    private static Dictionary<ComponentProperty, string> GetComponentProperties(ComponentType type) => type
        .GetPropertyNames()
        .ToDictionary(propertyName => propertyName, propertyName => propertyName.ToString());
}