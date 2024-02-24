using System.Diagnostics.CodeAnalysis;
using System.Management;
using System.Text.Json;
using DetectiveSpecs;

#pragma warning disable CA1869

var specs = Enum.GetValues<Component>()
    .Aggregate(new Dictionary<Component, Dictionary<string, string>>(),
               (specs, component) =>
               {
                   specs[component] = new Dictionary<string, string>();
                   string searchQuery = Queries.ForComponent(component);
                   ManagementObjectSearcher searcher = CreateManagementObjectSearcherByQuery(searchQuery);

                   foreach (ManagementBaseObject managementBaseObject in searcher.Get())
                       foreach (string key in component.GetSearchTerms())
                           if (TryGetValue(key, managementBaseObject, out string? value))
                               specs[component][key] = value;

                   return specs;
               });

string json = SerializeToJson(specs);
string? destinationPath = GetDestinationPath();

if (!HasAvailableSpace())
    throw new ApplicationException("Insufficient space for saving the results");

File.WriteAllText(destinationPath, json);
Console.WriteLine($"Saved computer specs to {destinationPath}.");
Console.WriteLine("Press a key to exit.");
Console.ReadKey();

return;

string GetDestinationPath()
{
    string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
    const string fileName = "ComputerInfo.json";

    return Path.Combine(currentDirectory, fileName);
}

bool TryGetValue(string key, ManagementBaseObject managementBaseObject, [NotNullWhen(true)] out string? value)
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

string SerializeToJson(Dictionary<Component, Dictionary<string, string>> dict)
{
    var options = new JsonSerializerOptions { WriteIndented = true };
    return JsonSerializer.Serialize(dict, options);
}

bool HasAvailableSpace()
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


ManagementObjectSearcher CreateManagementObjectSearcherByQuery(string query) => new(query);