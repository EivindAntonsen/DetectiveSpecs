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
        Component.Motherboard => ["Name", "Manufacturer", "Product", "SerialNumber"],
        Component.Gpu => ["Name", "AdapterRAM"],
        Component.Cpu => ["Name", "AdapterRAM"],
        Component.Storage => ["Model", "Type", "Partitions", "Size"],
        Component.Memory => ["Manufacturer","Model", "PartNumber", "Capacity", "Speed"],
        Component.Optical => ["Drive", "Manufacturer", "MediaType", "Status"],
        Component.Network => ["Name", "Description", "DeviceID", "MACAddress", "PhysicalAdapter"],
        Component.Sound => ["Name", "Manufacturer"],
        Component.Keyboard => ["Name", "Description", "Layout"],
        Component.Mouse => ["Name", "Manufacturer", "PointerType"],
        _ => throw new ArgumentOutOfRangeException(nameof(component), component, null)
    };
}