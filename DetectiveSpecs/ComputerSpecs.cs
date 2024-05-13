namespace DetectiveSpecs;

public record ComputerSpecs
{
    public required Component Motherboard { init; get; }
    public required IEnumerable<Component> Gpu { init; get; }
    public required Component Cpu { init; get; }
    public required IEnumerable<Component> Storage { init; get; }
    public required IEnumerable<Component> Memory { init; get; }
    public required IEnumerable<Component> Optical { init; get; }
    public required IEnumerable<Component> Network { init; get; }
    public required IEnumerable<Component> Sound { init; get; }
    public required IEnumerable<Component> Keyboard { init; get; }
    public required IEnumerable<Component> Mouse { init; get; }

    public IEnumerable<Component> GetAllComponents =>
        new List<Component> { Motherboard, Cpu }
            .Concat(Gpu)
            .Concat(Storage)
            .Concat(Memory)
            .Concat(Network)
            .Concat(Optical)
            .Concat(Sound)
            .Concat(Keyboard)
            .Concat(Mouse);
};