using System.Text.RegularExpressions;
using DetectiveSpecs.Enums;
using static DetectiveSpecs.Enums.ComponentProperty;
using static DetectiveSpecs.Enums.ComponentType;

namespace DetectiveSpecs;

public static partial class ComponentPropertyValueFormatter
{
    /// <summary>
    /// Formats a property value based on the component type and property.
    /// </summary>
    /// <param name="componentType">The type of the component.</param>
    /// <param name="key">The property of the component.</param>
    /// <param name="value">The value of the property.</param>
    /// <returns>The formatted property value as a string.</returns>
    public static string Format(ComponentType componentType, ComponentProperty key, object value) => componentType switch
    {
        Cpu when key is MaxClockSpeed && long.TryParse(value.ToString(), out var clockSpeedHertz) =>
            clockSpeedHertz / 1000 + " Ghz",
        Gpu when key is AdapterRAM && long.TryParse(value.ToString(), out var adapterRamBytes) =>
            Math.Round(BytesToGigaBytes(adapterRamBytes)) + " GB",
        Gpu when key is MinRefreshRate or MaxRefreshRate && !value.ToString()?.Contains(" Hz") is null or false =>
            value + " Hz",
        Gpu when key is VideoModeDescription =>
            GetFormattedVideoMode(value.ToString()!),
        Storage when key is Size && long.TryParse(value.ToString(), out var storageBytes) =>
            Math.Round(BytesToGigaBytes(storageBytes)) + " GB",
        Memory when key is Capacity && long.TryParse(value.ToString(), out var capacityBytes) =>
            Math.Round(BytesToGigaBytes(capacityBytes)) + " GB",
        Memory when key is Speed && long.TryParse(value.ToString(), out var memorySpeedHertz) =>
            memorySpeedHertz / 1000 + " Ghz",
        _ => value.ToString() ?? string.Empty
    };



    private static string GetFormattedAmount(long l) => l > 1_000_000_000 ? l / 1_000_000_000 + "b " : l / 1_000_000 + "m ";

    private static double BytesToGigaBytes(long bytes) => Convert.ToDouble(bytes) / 1024 / 1024 / 1024;



    private static string GetFormattedVideoMode(string videoMode)
    {
        var regex = ColorAmountRegex();

        var match = regex.Match(videoMode);
        if (!long.TryParse(match.Value, out var longValue))
            return string.Empty;

        var actualAmountOfColors = longValue / 256;
        var formattedAmount = GetFormattedAmount(actualAmountOfColors);

        return regex.Replace(videoMode, formattedAmount);
    }

    [GeneratedRegex(@"\d+\s+(?=colors$)")]
    private static partial Regex ColorAmountRegex();
}