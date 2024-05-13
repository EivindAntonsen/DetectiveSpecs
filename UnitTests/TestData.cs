using DetectiveSpecs;
using DetectiveSpecs.Enums;

namespace UnitTests;

public static class TestData
{
    /// <summary>
    /// Creates an instance of ComputerSpecs with default values.
    /// </summary>
    /// <returns>A new instance of the ComputerSpecs class.</returns>
    public static ComputerSpecs CreateComputerSpecs() => new()
    {
        Cpu = CreateComponent(ComponentType.Cpu),
        Gpu = [CreateComponent(ComponentType.Gpu)],
        Motherboard = CreateComponent(ComponentType.Motherboard),
        Storage = [CreateComponent(ComponentType.Storage)],
        Memory = [CreateComponent(ComponentType.Memory)],
        Optical = [CreateComponent(ComponentType.Optical)],
        Network = [CreateComponent(ComponentType.Network)],
        Sound = [CreateComponent(ComponentType.Sound)],
        Keyboard = [CreateComponent(ComponentType.Keyboard)],
        Mouse = [CreateComponent(ComponentType.Mouse)]
    };



    private static Component CreateComponent(ComponentType componentType) =>
        new(componentType, GetComponentProperties(componentType));



    private static Dictionary<ComponentProperty, string> GetComponentProperties(ComponentType type) => type
        .GetPropertyNames()
        .ToDictionary(propertyName => propertyName, propertyName => propertyName.ToString());
}