using System.Diagnostics.CodeAnalysis;
using System.Management;
using System.Text.Json;
using DetectiveSpecs.Models;

#pragma warning disable CA1869

namespace DetectiveSpecs;

internal static class Program
{
    private static IEnumerable<Component> GetMultipleComponentsOfType(ComponentType componentType)
    {
        LogComponentSearch(componentType);
        
        var queryString = Queries.ForComponent(componentType);
        var searcher = new ManagementObjectSearcher(queryString);

        foreach (var managementBaseObject in searcher.Get())
        {
            var properties = new Dictionary<ComponentProperty, string>();

            foreach (var propertyName in componentType.GetPropertyNames())
                if (TryGetValue(propertyName, managementBaseObject, out var propertyValue))
                    properties.Add(propertyName, propertyValue);

            yield return new Component(properties);
        }
    }



    private static Component? GetComponentOfType(ComponentType componentType)
    {
        LogComponentSearch(componentType);
        
        var queryString = Queries.ForComponent(componentType);
        var searcher = new ManagementObjectSearcher(queryString);

        var managementBaseObject = searcher.Get()
            .OfType<ManagementBaseObject>()
            .FirstOrDefault();

        if (managementBaseObject is null)
            return null;

        var properties = new Dictionary<ComponentProperty, string>();

        foreach (var propertyName in componentType.GetPropertyNames())
            if (TryGetValue(propertyName, managementBaseObject, out var propertyValue) && !string.IsNullOrWhiteSpace(propertyValue))
                properties.Add(propertyName, propertyValue);

        return new Component(properties);
    }



    public static async Task Main(string[] args)
    {
        Console.WriteLine("Starting work on detecting components");

        var computerSpecs = GetComputerSpecs();
        var destinationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ComputerInfo");
        var yamlPath = await FileWriter.WriteAsYaml(computerSpecs, destinationPath).ConfigureAwait(false);
        var jsonPath = await FileWriter.WriteAsJson(computerSpecs, destinationPath).ConfigureAwait(false);
        var paths = new List<string> { yamlPath, jsonPath };

        Console.WriteLine($"Saved computer specs to {JsonSerializer.Serialize(paths)}.");
        Console.WriteLine("Press a key to exit.");
        Console.ReadKey();
    }



    private static ComputerSpecs GetComputerSpecs() => new()
    {
        Cpu = GetComponentOfType(ComponentType.Cpu)
              ?? throw new ApplicationException($"Unable to find information about the {nameof(ComponentType.Cpu)}"),
        Gpu = GetMultipleComponentsOfType(ComponentType.Gpu),
        Motherboard = GetComponentOfType(ComponentType.Motherboard)
                      ?? throw new ApplicationException($"Unable to find information about the {nameof(ComponentType.Motherboard)}"),
        Storage = GetMultipleComponentsOfType(ComponentType.Storage),
        Memory = GetMultipleComponentsOfType(ComponentType.Memory),
        Optical = GetMultipleComponentsOfType(ComponentType.Optical),
        Network = GetMultipleComponentsOfType(ComponentType.Network)
            .Where(component => component.Properties[ComponentProperty.PhysicalAdapter] == "True"),
        Sound = GetMultipleComponentsOfType(ComponentType.Sound),
        Keyboard = GetMultipleComponentsOfType(ComponentType.Keyboard),
        Mouse = GetMultipleComponentsOfType(ComponentType.Mouse),
    };



    private static bool TryGetValue(ComponentProperty key, ManagementBaseObject managementBaseObject, [NotNullWhen(true)] out string? value)
    {
        try
        {
            var information = managementBaseObject[key.ToString()];
            value = Convert.ToString(information)?.Trim() ?? string.Empty;
            return true;
        }
        catch (ManagementException exception) when (exception.ErrorCode == ManagementStatus.NotFound)
        {
            value = string.Empty;
            return false;
        }
    }



    private static void LogComponentSearch(ComponentType componentType)
    {
        Console.Write("Searching for information about ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"{componentType}");
        Console.ResetColor();
        Console.WriteLine();
    }
}