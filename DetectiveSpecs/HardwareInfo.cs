using DetectiveSpecs.Enums;

namespace DetectiveSpecs;

public record HardwareInfo
{
    public HardwareInfo(IEnumerable<Component> components)
    {
        foreach (var component in components.GroupBy(component => component.ComponentType))
        {
            switch (component.Key)
            {
                case ComponentType.Motherboard:
                    Motherboard = component;
                    break;
                case ComponentType.Gpu:
                    Gpu = component;
                    break;
                case ComponentType.Cpu:
                    Cpu = component;
                    break;
                case ComponentType.Storage:
                    Storage = component;
                    break;
                case ComponentType.Memory:
                    Memory = component;
                    break;
                case ComponentType.Optical:
                    Optical = component;
                    break;
                case ComponentType.Network:
                    Network = component;
                    break;
                case ComponentType.Sound:
                    Sound = component;
                    break;
                case ComponentType.Keyboard:
                    Keyboard = component;
                    break;
                case ComponentType.Mouse:
                    Mouse = component;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }



    public IEnumerable<Component> Motherboard { get; } = new List<Component>();
    public IEnumerable<Component> Gpu { get; } = new List<Component>();
    public IEnumerable<Component> Cpu { get; } = new List<Component>();
    public IEnumerable<Component> Storage { get; } = new List<Component>();
    public IEnumerable<Component> Memory { get; } = new List<Component>();
    public IEnumerable<Component> Optical { get; } = new List<Component>();
    public IEnumerable<Component> Network { get; } = new List<Component>();
    public IEnumerable<Component> Sound { get; } = new List<Component>();
    public IEnumerable<Component> Keyboard { get; } = new List<Component>();
    public IEnumerable<Component> Mouse { get; } = new List<Component>();

    public IEnumerable<Component> GetAllComponents => Motherboard
        .Concat(Cpu)
        .Concat(Gpu)
        .Concat(Storage)
        .Concat(Memory)
        .Concat(Network)
        .Concat(Optical)
        .Concat(Sound)
        .Concat(Keyboard)
        .Concat(Mouse);
};