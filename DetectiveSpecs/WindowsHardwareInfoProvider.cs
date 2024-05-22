using System.Diagnostics.CodeAnalysis;
using System.Management;
using DetectiveSpecs.Enums;

namespace DetectiveSpecs;

public static class WindowsHardwareInfoProvider
{
    public static IEnumerable<Component> GetComponents(ComponentType componentType)
    {
        var queryString = Queries.ForComponent(componentType);
        var searcher = new ManagementObjectSearcher(queryString);

        foreach (var managementBaseObject in searcher.Get())
        {
            var properties = ReadComponentProperties(componentType, managementBaseObject);

            yield return new Component(componentType, properties);
        }
    }



    private static Dictionary<ComponentProperty, string> ReadComponentProperties(
        ComponentType componentType,
        ManagementBaseObject managementObject)
    {
        Console.WriteLine($"Reading component properties of {componentType}.");

        return componentType
            .GetPropertyNames()
            .Aggregate(new Dictionary<ComponentProperty, string>(), (properties, key) =>
            {
                if (!TryGetValue(key, managementObject, out var value) || string.IsNullOrWhiteSpace(value))
                    return properties;

                var formattedValue = ComponentPropertyValueFormatter.Format(componentType, key, value);

                properties.Add(key, formattedValue);

                return properties;
            });
    }



    private static bool TryGetValue(
        ComponentProperty key,
        ManagementBaseObject managementBaseObject,
        [NotNullWhen(true)] out string? value)
    {
        try
        {
            var information = managementBaseObject[key.ToString()];
            value = Convert.ToString(information)?.Trim();
            return value is not null;
        }
        catch (ManagementException exception) when (exception.ErrorCode == ManagementStatus.NotFound)
        {
            value = null;
            return false;
        }
    }
}