using DetectiveSpecs.Enums;
using static DetectiveSpecs.Enums.ComponentProperty;
using static DetectiveSpecs.Enums.ComponentType;

namespace DetectiveSpecs;

internal static class Program
{
    public static async Task Main()
    {
        Console.WriteLine("Starting work on detecting components");

        var computerSpecs = GetHardwareInfo();
        var serializedText = new HardwareInfoSerializer().Serialize(computerSpecs);
        var currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var path = Path.Combine(currentDirectory, "ComputerInfo.txt");

        await File.WriteAllTextAsync(path, serializedText).ConfigureAwait(false);

        Console.WriteLine($"Saved computer specs to {path}");
        Console.WriteLine("Press a key to exit.");
        Console.ReadKey();
    }



    private static HardwareInfo GetHardwareInfo()
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
}