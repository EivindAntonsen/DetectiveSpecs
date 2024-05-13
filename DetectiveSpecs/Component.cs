using DetectiveSpecs.Enums;

namespace DetectiveSpecs;

public record Component(ComponentType ComponentType, Dictionary<ComponentProperty, object> Properties);