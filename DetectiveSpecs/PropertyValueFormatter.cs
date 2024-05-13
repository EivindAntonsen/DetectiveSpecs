using System.Text.RegularExpressions;
using DetectiveSpecs.Enums;

namespace DetectiveSpecs;

public static class PropertyValueFormatter
{
    public static object? Format(ComponentType componentType, ComponentProperty componentProperty, string? propertyValue) => componentType switch
    {
        ComponentType.Cpu when componentProperty is ComponentProperty.MaxClockSpeed && long.TryParse(propertyValue, out var clockSpeedHertz) =>
            clockSpeedHertz / 1000 + " Ghz",
        ComponentType.Gpu when componentProperty is ComponentProperty.AdapterRAM && long.TryParse(propertyValue, out var adapterRamBytes) =>
            Math.Round(Convert.ToDouble(adapterRamBytes) / 1024 / 1024 / 1024) + " GB",
        ComponentType.Gpu when componentProperty is ComponentProperty.MinRefreshRate or ComponentProperty.MaxRefreshRate =>
            propertyValue + " Hz",
        ComponentType.Gpu when componentProperty is ComponentProperty.VideoModeDescription && propertyValue is not null =>
            GetFormattedVideoMode(propertyValue),
        ComponentType.Storage when componentProperty is ComponentProperty.Size && long.TryParse(propertyValue, out var storageBytes) =>
            Math.Round(Convert.ToDouble(storageBytes) / 1024 / 1024 / 1024) + " GB",
        ComponentType.Memory when componentProperty is ComponentProperty.Capacity && long.TryParse(propertyValue, out var capacityBytes) =>
            Math.Round(Convert.ToDouble(capacityBytes) / 1024 / 1024 / 1024) + " GB",
        ComponentType.Memory when componentProperty is ComponentProperty.Speed && long.TryParse(propertyValue, out var memorySpeedHertz) =>
            memorySpeedHertz / 1000 + " Ghz",
        _ => propertyValue
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