using static DetectiveSpecs.Models.ComponentType;
using static DetectiveSpecs.Models.ComponentProperty;

namespace DetectiveSpecs.Models;

public enum ComponentType
{
    Motherboard,
    Gpu,
    Cpu,
    Storage,
    Memory,
    Optical,
    Network,
    Sound,
    Keyboard,
    Mouse
}

public static class ComponentExtensions
{
    public static IEnumerable<ComponentProperty> GetPropertyNames(this ComponentType componentType) => componentType switch
    {
        Motherboard => [Name, Manufacturer, Product, SerialNumber],
        Gpu => [Name, AdapterRAM],
        Cpu => [Name, AdapterRAM],
        Storage => [Model, ComponentProperty.Type, Partitions, Size],
        Memory => [Manufacturer, Model, PartNumber, Capacity, Speed],
        Optical => [Drive, Manufacturer, MediaType, Status],
        Network => [Name, Description, DeviceID, MACAddress, PhysicalAdapter],
        Sound => [Name, Manufacturer],
        Keyboard => [Name, Description, Layout],
        Mouse => [Name, Manufacturer, PointerType],
        _ => throw new ArgumentOutOfRangeException(nameof(componentType), componentType, null)
    };
}