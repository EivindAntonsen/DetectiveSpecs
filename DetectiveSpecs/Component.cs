namespace DetectiveSpecs;

public enum Component
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
    public static IEnumerable<string> GetSearchTerms(this Component component) => component switch
    {
        Component.Motherboard => ["Name", "Manufacturer", "Product", "SerialNumber", "Version"],
        Component.Gpu => ["Name", "AdapterRAM"],
        Component.Cpu => ["Name", "AdapterRAM"],
        Component.Storage => ["Name", "Model", "Type", "Partitions", "Size", "Serial Number"],
        Component.Memory => ["Name", "Manufacturer", "PartNumber", "Capacity", "Speed"],
        Component.Optical => ["Drive", "Manufacturer", "MediaType", "Status"],
        Component.Network => ["Name", "Description", "DeviceID", "MACAddress", "PhysicalAdapter"],
        Component.Sound => ["Name", "Manufacturer", "Status"],
        Component.Keyboard => ["Name", "Description", "Layout"],
        Component.Mouse => ["Name", "Manufacturer", "PointerType"],
        _ => throw new ArgumentOutOfRangeException(nameof(component), component, null)
    };
}