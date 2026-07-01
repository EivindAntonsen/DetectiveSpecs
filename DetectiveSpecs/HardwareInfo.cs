using DetectiveSpecs.Enums;

namespace DetectiveSpecs;

public record HardwareInfo
{
    private readonly Dictionary<ComponentType, List<Component>> _components;

    public HardwareInfo(params IEnumerable<Component>[] arraysOfComponents)
        : this(arraysOfComponents.SelectMany(x => x)) { }

    public HardwareInfo(IEnumerable<Component> components)
    {
        _components = components
            .GroupBy(c => c.ComponentType)
            .ToDictionary(g => g.Key, g => g.ToList());
    }

    private IEnumerable<Component> Get(ComponentType type) =>
        _components.TryGetValue(type, out var list) ? list : [];

    public IEnumerable<Component> Motherboard => Get(ComponentType.Motherboard);
    public IEnumerable<Component> Gpu => Get(ComponentType.Gpu);
    public IEnumerable<Component> Cpu => Get(ComponentType.Cpu);
    public IEnumerable<Component> Storage => Get(ComponentType.Storage);
    public IEnumerable<Component> Memory => Get(ComponentType.Memory);
    public IEnumerable<Component> Optical => Get(ComponentType.Optical);
    public IEnumerable<Component> Network => Get(ComponentType.Network);
    public IEnumerable<Component> Sound => Get(ComponentType.Sound);
    public IEnumerable<Component> Keyboard => Get(ComponentType.Keyboard);
    public IEnumerable<Component> Mouse => Get(ComponentType.Mouse);
    public IEnumerable<Component> OperatingSystem => Get(ComponentType.OperatingSystem);

    public IEnumerable<Component> GetAllComponents => [
        .. Motherboard,
        .. OperatingSystem,
        .. Cpu,
        .. Gpu,
        .. Storage,
        .. Memory,
        .. Network,
        .. Optical,
        .. Sound,
        .. Keyboard,
        .. Mouse
    ];
}