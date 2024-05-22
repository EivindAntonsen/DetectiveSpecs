using DetectiveSpecs.Enums;
using static DetectiveSpecs.Enums.ComponentType;

namespace DetectiveSpecs;

internal static class Queries
{
    private static readonly Dictionary<ComponentType, string> QueriesByComponentType = new()
    {
        { Gpu, "select * from Win32_VideoController" },
        { Motherboard, "select * from Win32_BaseBoard" },
        { Cpu, "select * from Win32_Processor" },
        { Storage, "select * from Win32_DiskDrive" },
        { Memory, "select * from Win32_PhysicalMemory" },
        { Optical, "SELECT * FROM Win32_CDROMDrive" },
        { Network, "SELECT * FROM Win32_NetworkAdapter" },
        { Sound, "SELECT * FROM Win32_SoundDevice" },
        { Keyboard, "SELECT * FROM Win32_Keyboard" },
        { Mouse, "SELECT * FROM Win32_PointingDevice" },
        { ComponentType.OperatingSystem, "SELECT * FROM Win32_OperatingSystem" }
    };


    public static string ForComponent(ComponentType componentType) => QueriesByComponentType[componentType];
}