using static DetectiveSpecs.Enums.ComponentType;
using static DetectiveSpecs.Enums.ComponentProperty;

namespace DetectiveSpecs.Enums;

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
    Mouse,
    OperatingSystem
}

public static class ComponentExtensions
{
    public static IEnumerable<ComponentProperty> GetPropertyNames(this ComponentType componentType) => componentType switch
    {
        Motherboard => [Name, Manufacturer, Product, SerialNumber],
        Gpu => [Name, MinRefreshRate, MaxRefreshRate, AdapterRAM, VideoModeDescription],
        Cpu => [Name, AdapterRAM, MaxClockSpeed, NumberOfCores],
        Storage => [Description, MediaType, Model, ComponentProperty.Type, Partitions, Size],
        Memory => [DeviceLocator, Manufacturer, Model, PartNumber, Capacity, Speed, SerialNumber],
        Optical => [Drive, Manufacturer, MediaType, Status],
        Network => [Name, Description, DeviceID, MACAddress, PhysicalAdapter],
        Sound => [Name, Manufacturer],
        Keyboard => [Name, Description, Layout, NumberOfFunctionKeys],
        Mouse => [Name, Manufacturer, PointerType],
        ComponentType.OperatingSystem => [Caption, ComponentProperty.Version, Manufacturer, OSArchitecture, Status],
        _ => throw new ArgumentOutOfRangeException(nameof(componentType), componentType, null)
    };
}