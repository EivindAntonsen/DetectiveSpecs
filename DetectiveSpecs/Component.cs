using DetectiveSpecs.Enums;

namespace DetectiveSpecs;

public record Component(ComponentType ComponentType, Dictionary<ComponentProperty, string> Properties)
{
    public string? GetProperty(ComponentProperty property) =>
        Properties.TryGetValue(property, out var value) ? value : null;
}