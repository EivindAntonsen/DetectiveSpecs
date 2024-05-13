using System.Text.RegularExpressions;
using DetectiveSpecs.Enums;

namespace DetectiveSpecs;

public static class ComponentPropertyValueFormatter
{
    /// <summary>
    /// Formats a property value based on the component type and property.
    /// </summary>
    /// <param name="componentType">The type of the component.</param>
    /// <param name="componentProperty">The property of the component.</param>
    /// <param name="propertyValue">The value of the property.</param>
    /// <returns>The formatted property value as a string.</returns>
    public static string Format(ComponentType componentType, ComponentProperty componentProperty, object propertyValue) => componentType switch
    {
        ComponentType.Cpu when componentProperty is ComponentProperty.MaxClockSpeed && long.TryParse(propertyValue.ToString(), out var clockSpeedHertz) =>
            clockSpeedHertz / 1000 + " Ghz",
        ComponentType.Gpu when componentProperty is ComponentProperty.AdapterRAM && long.TryParse(propertyValue.ToString(), out var adapterRamBytes) =>
            Math.Round(Convert.ToDouble(adapterRamBytes) / 1024 / 1024 / 1024) + " GB",
        ComponentType.Gpu when componentProperty is ComponentProperty.MinRefreshRate or ComponentProperty.MaxRefreshRate =>
            propertyValue + " Hz",
        ComponentType.Gpu when componentProperty is ComponentProperty.VideoModeDescription =>
            GetFormattedVideoMode(propertyValue.ToString()!),
        ComponentType.Storage when componentProperty is ComponentProperty.Size && long.TryParse(propertyValue.ToString(), out var storageBytes) =>
            Math.Round(Convert.ToDouble(storageBytes) / 1024 / 1024 / 1024) + " GB",
        ComponentType.Memory when componentProperty is ComponentProperty.Capacity && long.TryParse(propertyValue.ToString(), out var capacityBytes) =>
            Math.Round(Convert.ToDouble(capacityBytes) / 1024 / 1024 / 1024) + " GB",
        ComponentType.Memory when componentProperty is ComponentProperty.Speed && long.TryParse(propertyValue.ToString(), out var memorySpeedHertz) =>
            memorySpeedHertz / 1000 + " Ghz",
        _ => propertyValue.ToString() ?? string.Empty
    };



    private static string GetFormattedAmount(long l) => l > 1_000_000_000 ? l / 1_000_000_000 + "b " : l / 1_000_000 + "m ";



    private static string GetFormattedVideoMode(string videoMode)
    {
        var regex = new Regex(@"\d+\s+(?=colors$)");

        Match match = regex.Match(videoMode);
        if (!long.TryParse(match.Value, out var longValue))
            return string.Empty;

        var actualAmountOfColors = longValue / 256;
        var formattedAmount = GetFormattedAmount(actualAmountOfColors);

        return regex.Replace(videoMode, formattedAmount);
    }
}