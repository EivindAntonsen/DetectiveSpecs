using System.Diagnostics.CodeAnalysis;
using System.Management;
using System.Text.Json;

#pragma warning disable CA1869

namespace DetectiveSpecs;

internal static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Starting work on detecting components");
        var specs = Enum.GetValues<Component>()
            .Aggregate(new Dictionary<Component, List<Dictionary<string, string>>>(),
                       (specs, component) =>
                       {
                           Console.Write("Searching for information about ");
                           Console.ForegroundColor = ConsoleColor.Yellow;
                           Console.Write($"{component}");
                           Console.ResetColor();
                           Console.WriteLine();
                           specs[component] = [];

                           string searchQuery = Queries.ForComponent(component);
                           ManagementObjectSearcher searcher = CreateManagementObjectSearcherByQuery(searchQuery);

                           foreach (ManagementBaseObject managementBaseObject in searcher.Get())
                           {
                               var dictionary = component.GetSearchTerms()
                                   .Aggregate(new Dictionary<string, string>(),
                                              (dictionary, key) =>
                                              {
                                                  if (TryGetValue(key, managementBaseObject, out string? value))
                                                      dictionary[key] = value;

                                                  return dictionary;
                                              });

                               Console.Write("[");
                               Console.ForegroundColor = ConsoleColor.DarkCyan;
                               Console.Write($"{dictionary.GetValueOrDefault("Name")}");
                               Console.ResetColor();
                               Console.Write($"]: Found {dictionary.Count} component properties");
                               Console.WriteLine();

                               specs[component].Add(dictionary);
                           }

                           return specs;
                       });

        RemoveNonPhysicalNetworkAdapters(ref specs);

        string json = SerializeToJson(specs);
        string? destinationPath = GetDestinationPath();

        if (!HasAvailableSpace(destinationPath, json))
            throw new ApplicationException("Insufficient space for saving the results");

        File.WriteAllText(destinationPath, json);
        Console.WriteLine($"Saved computer specs to {destinationPath}.");
        Console.WriteLine("Press a key to exit.");
        Console.ReadKey();

        return;
    }



    private static string GetDestinationPath()
    {
        string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        const string fileName = "ComputerInfo.json";

        return Path.Combine(currentDirectory, fileName);
    }



    private static bool TryGetValue(string key, ManagementBaseObject managementBaseObject, [NotNullWhen(true)] out string? value)
    {
        try
        {
            object information = managementBaseObject[key];
            value = Convert.ToString(information)?.Trim() ?? string.Empty;
            return true;
        }
        catch (ManagementException exception) when (exception.ErrorCode == ManagementStatus.NotFound)
        {
            value = string.Empty;
            return false;
        }
    }



    private static string SerializeToJson(object dict)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        return JsonSerializer.Serialize(dict, options);
    }



    private static bool HasAvailableSpace(string destinationPath, string json)
    {
        Console.WriteLine("Checking for available space");
        var driveInfo = new DriveInfo(Path.GetPathRoot(destinationPath)!);
        int requiredSpace = json.Length * sizeof(char);

        if (driveInfo.AvailableFreeSpace >= requiredSpace)
        {
            Console.WriteLine("Found sufficient space for saving results");
            return true;
        }

        Console.WriteLine("Insufficient space for saving results");
        return false;
    }



    private static void RemoveNonPhysicalNetworkAdapters(ref Dictionary<Component, List<Dictionary<string, string>>> dict)
    {
        dict[Component.Network].RemoveAll(dictionary =>
                                              dictionary.TryGetValue("PhysicalAdapter", out string? isPhysical) && isPhysical == "False");
    }



    private static ManagementObjectSearcher CreateManagementObjectSearcherByQuery(string query) => new(query);
}