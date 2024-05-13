using System.Diagnostics.CodeAnalysis;
using System.Management;
using DetectiveSpecs.Enums;

namespace DetectiveSpecs;

internal static class Program
{
    public static async Task Main()
    {
        Console.WriteLine("Starting work on detecting components");

        var computerSpecs = GetComputerSpecs();
        var serializedText = new ComputerSpecSerializer().Serialize(computerSpecs);
        var currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var path = Path.Combine(currentDirectory, "ComputerInfo.txt");

        await File.WriteAllTextAsync(path, serializedText).ConfigureAwait(false);

        Console.WriteLine($"Saved computer specs to {path}");
        Console.WriteLine("Press a key to exit.");
        Console.ReadKey();
    }



    private static ComputerSpecs GetComputerSpecs()
    {
        var cpu = GetComponentOfType(ComponentType.Cpu)
                  ?? throw new ApplicationException($"Unable to find information about the {nameof(ComponentType.Cpu)}");
        var gpu = GetMultipleComponentsOfType(ComponentType.Gpu);
        var motherBoard = GetComponentOfType(ComponentType.Motherboard)
                          ?? throw new ApplicationException($"Unable to find information about the {nameof(ComponentType.Motherboard)}");
        var storage = GetMultipleComponentsOfType(ComponentType.Storage);
        var memory = GetMultipleComponentsOfType(ComponentType.Memory);
        var optical = GetMultipleComponentsOfType(ComponentType.Optical);
        var network = GetMultipleComponentsOfType(ComponentType.Network)
            .Where(component => (string) component.Properties[ComponentProperty.PhysicalAdapter] == "True");
        var sound = GetMultipleComponentsOfType(ComponentType.Sound);
        var keyboard = GetMultipleComponentsOfType(ComponentType.Keyboard);
        var mouse = GetMultipleComponentsOfType(ComponentType.Mouse);

        return new ComputerSpecs
        {
            Cpu = cpu,
            Gpu = gpu,
            Motherboard = motherBoard,
            Storage = storage,
            Memory = memory,
            Optical = optical,
            Network = network,
            Sound = sound,
            Keyboard = keyboard,
            Mouse = mouse
        };
    }



    private static Dictionary<ComponentProperty, object> ReadComponentProperties(
        ComponentType componentType,
        ManagementBaseObject managementObject)
    {
        Console.WriteLine($"Reading component properties of {componentType}.");

        return componentType
            .GetPropertyNames()
            .Aggregate(new Dictionary<ComponentProperty, object>(), (properties, key) =>
            {
                if (!TryGetValue(key, managementObject, out var value) || string.IsNullOrWhiteSpace(value))
                    return properties;

                object? formattedValue = PropertyValueFormatter.Format(componentType, key, value);

                if (formattedValue is null)
                    return properties;

                properties.Add(key, formattedValue);

                return properties;
            });
    }



    private static IEnumerable<Component> GetMultipleComponentsOfType(ComponentType componentType)
    {
        var queryString = Queries.ForComponent(componentType);
        var searcher = new ManagementObjectSearcher(queryString);

        foreach (var managementBaseObject in searcher.Get())
        {
            var properties = ReadComponentProperties(componentType, managementBaseObject);

            yield return new Component(componentType, properties);
        }
    }



    private static Component? GetComponentOfType(ComponentType componentType)
    {
        var queryString = Queries.ForComponent(componentType);
        var searcher = new ManagementObjectSearcher(queryString);

        var searchResult = searcher.Get()
            .OfType<ManagementBaseObject>()
            .FirstOrDefault();

        if (searchResult is null)
            return null;

        var properties = ReadComponentProperties(componentType, searchResult);

        return new Component(componentType, properties);
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