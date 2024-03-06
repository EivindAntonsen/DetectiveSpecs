﻿using System.Diagnostics.CodeAnalysis;
using System.Management;
using System.Text.Json;
using DetectiveSpecs.Models;

// ReSharper disable SwitchStatementMissingSomeEnumCasesNoDefault

#pragma warning disable CA1869

namespace DetectiveSpecs;

internal static class Program
{
    public static async Task Main()
    {
        Console.WriteLine("Starting work on detecting components");

        var computerSpecs = GetComputerSpecs();

        var destinationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ComputerInfo");
        var formattedText = ComputerSpecSerializer.Serialize(computerSpecs, destinationPath);
        
        await File.WriteAllTextAsync(destinationPath, formattedText).ConfigureAwait(false);

        Console.WriteLine($"Saved computer specs to {formattedText}.");
        Console.WriteLine("Press a key to exit.");
        Console.ReadKey();
    }



    private static Dictionary<ComponentProperty, string> ReadComponentProperties(ComponentType componentType, ManagementBaseObject managementObject) => componentType
        .GetPropertyNames()
        .Aggregate(new Dictionary<ComponentProperty, string>(), (properties, propertyName) =>
        {
            if (TryGetValue(propertyName, managementObject, out var propertyValue) && !string.IsNullOrWhiteSpace(propertyValue))
                properties.Add(propertyName, propertyValue);

            return properties;
        });



    private static IEnumerable<Component> GetMultipleComponentsOfType(ComponentType componentType)
    {
        var queryString = Queries.ForComponent(componentType);
        var searcher = new ManagementObjectSearcher(queryString);

        foreach (var managementBaseObject in searcher.Get())
        {
            var properties = ReadComponentProperties(componentType, managementBaseObject);

            yield return new Component(properties);
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

        return new Component(properties);
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