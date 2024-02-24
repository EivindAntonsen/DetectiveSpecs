namespace DetectiveSpecs;

internal static class Queries
{
    private const string GpuQuery = "select * from Win32_VideoController";
    private const string MoboQuery = "select * from Win32_BaseBoard";
    private const string CpuQuery = "select * from Win32_Processor";
    private const string DiskQuery = "select * from Win32_DiskDrive";
    private const string MemoryQuery = "select * from Win32_PhysicalMemory";
    private const string OpticalQuery = "SELECT * FROM Win32_CDROMDrive";
    private const string NetworkQuery = "SELECT * FROM Win32_NetworkAdapter";
    private const string SoundQuery = "SELECT * FROM Win32_SoundDevice";
    private const string KeyboardQuery = "SELECT * FROM Win32_Keyboard";
    private const string MouseQuery = "SELECT * FROM Win32_PointingDevice";



    public static string ForComponent(Component component) => component switch
    {
        Component.Motherboard => MoboQuery,
        Component.Gpu => GpuQuery,
        Component.Cpu => CpuQuery,
        Component.Storage => DiskQuery,
        Component.Memory => MemoryQuery,
        Component.Optical => OpticalQuery,
        Component.Network => NetworkQuery,
        Component.Sound => SoundQuery,
        Component.Keyboard => KeyboardQuery,
        Component.Mouse => MouseQuery,
        _ => throw new ArgumentOutOfRangeException(nameof(component))
    };
}