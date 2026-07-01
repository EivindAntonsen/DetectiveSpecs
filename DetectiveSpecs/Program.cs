using System.CommandLine;
using DetectiveSpecs;
using DetectiveSpecs.Enums;
using static DetectiveSpecs.Enums.ComponentProperty;
using static DetectiveSpecs.Enums.ComponentType;

var fileOption = new Option<FileInfo?>(
    name: "--output",
    description: "The file to write the hardware information to.");
fileOption.AddAlias("-o");

var consoleOption = new Option<bool>(
    name: "--console",
    description: "Print the hardware information to the console.");
consoleOption.AddAlias("-c");

var rootCommand = new RootCommand("DetectiveSpecs - A tool to detect and serialize computer hardware information.");
rootCommand.AddOption(fileOption);
rootCommand.AddOption(consoleOption);

rootCommand.SetHandler(async (outputFile, printToConsole) =>
{
    Console.WriteLine("Starting work on detecting components...");

    var computerSpecs = GetHardwareInfo();
    var serializedText = new HardwareInfoSerializer().Serialize(computerSpecs);

    if (printToConsole)
    {
        Console.WriteLine(serializedText);
    }

    if (outputFile != null)
    {
        await File.WriteAllTextAsync(outputFile.FullName, serializedText).ConfigureAwait(false);
        Console.WriteLine($"Saved computer specs to {outputFile.FullName}");
    }
    else if (!printToConsole)
    {
        // Default behavior if no options provided: save to default file
        var currentDirectory = Directory.GetCurrentDirectory();
        var path = Path.Combine(currentDirectory, "ComputerInfo.txt");
        await File.WriteAllTextAsync(path, serializedText).ConfigureAwait(false);
        Console.WriteLine($"Saved computer specs to {path}");
    }

}, fileOption, consoleOption);

return await rootCommand.InvokeAsync(args);

static HardwareInfo GetHardwareInfo()
{
    var cpu = WindowsHardwareInfoProvider.GetComponents(Cpu).ToList();
    var gpu = WindowsHardwareInfoProvider.GetComponents(Gpu).ToList();
    var motherBoard = WindowsHardwareInfoProvider.GetComponents(Motherboard).ToList();
    var storage = WindowsHardwareInfoProvider.GetComponents(Storage).ToList();
    var memory = WindowsHardwareInfoProvider.GetComponents(Memory).ToList();
    var optical = WindowsHardwareInfoProvider.GetComponents(Optical).ToList();
    var network = WindowsHardwareInfoProvider.GetComponents(Network)
        .Where(component => component.Properties.TryGetValue(PhysicalAdapter, out string? value) && value == "True").ToList();
    var sound = WindowsHardwareInfoProvider.GetComponents(Sound).ToList();
    var keyboard = WindowsHardwareInfoProvider.GetComponents(Keyboard).ToList();
    var mouse = WindowsHardwareInfoProvider.GetComponents(Mouse).ToList();
    var operatingSystem = WindowsHardwareInfoProvider.GetComponents(ComponentType.OperatingSystem).ToList();

    return new HardwareInfo(motherBoard,
        operatingSystem,
        cpu,
        gpu,
        storage,
        memory,
        optical,
        network,
        sound,
        keyboard,
        mouse);
}