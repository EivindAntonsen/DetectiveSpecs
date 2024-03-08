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
        Cpu = new Component(ComponentType.Cpu.GetPropertyNames().ToDictionary(property => property, CreateComponentPropertyValue)),
        Gpu = [new Component(ComponentType.Gpu.GetPropertyNames().ToDictionary(property => property, CreateComponentPropertyValue))],
        Motherboard = new Component(ComponentType.Motherboard.GetPropertyNames().ToDictionary(property => property, CreateComponentPropertyValue)),
        Storage = [new Component(ComponentType.Storage.GetPropertyNames().ToDictionary(property => property, CreateComponentPropertyValue))],
        Memory = [new Component(ComponentType.Memory.GetPropertyNames().ToDictionary(property => property, CreateComponentPropertyValue))],
        Optical = [new Component(ComponentType.Optical.GetPropertyNames().ToDictionary(property => property, CreateComponentPropertyValue))],
        Network = [new Component(ComponentType.Network.GetPropertyNames().ToDictionary(property => property, CreateComponentPropertyValue))],
        Sound = [new Component(ComponentType.Sound.GetPropertyNames().ToDictionary(property => property, CreateComponentPropertyValue))],
        Keyboard = [new Component(ComponentType.Keyboard.GetPropertyNames().ToDictionary(property => property, CreateComponentPropertyValue))],
        Mouse = [new Component(ComponentType.Mouse.GetPropertyNames().ToDictionary(property => property, CreateComponentPropertyValue))]
    };



    public static Component CreateComponent(Dictionary<ComponentProperty, string> properties) =>
        new(properties);



    public static string CreateComponentPropertyValue(ComponentProperty property) =>
        property.ToString();
}