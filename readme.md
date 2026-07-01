# DetectiveSpecs

[![CI/CD](https://github.com/EivindAntonsen/DetectiveSpecs/actions/workflows/CI.yaml/badge.svg)](https://github.com/EivindAntonsen/DetectiveSpecs/actions/workflows/CI.yaml)
[![NuGet](https://img.shields.io/nuget/v/DetectiveSpecs.svg)](https://www.nuget.org/packages/DetectiveSpecs/)

A modern .NET tool to detect computer hardware information and serialize it.

## Installation

### As a .NET Tool (Recommended)
You can install DetectiveSpecs as a global tool using the .NET SDK:
```bash
dotnet tool install -g DetectiveSpecs
```

### As a Standalone Executable
Download the latest `DetectiveSpecs.exe` from the [Releases](https://github.com/EivindAntonsen/DetectiveSpecs/releases) page. It is a self-contained single-file executable for Windows x64.

## Usage

Run the tool using `detective-specs` (if installed as a tool) or by executing the binary.

```bash
# Print hardware info to console
detective-specs --console

# Save hardware info to a specific file
detective-specs --output MyPC.txt

# Save to default file (ComputerInfo.txt in current directory)
detective-specs
```

### Options
- `-c, --console`: Print the hardware information directly to the console.
- `-o, --output <file>`: The file to write the hardware information to.

## Features
- **Modern .NET 9**: Built with the latest C# features and performance improvements.
- **Declarative Style**: Maintains a clean, declarative approach to hardware detection.
- **Self-Contained**: Available as a single-file EXE with no dependencies required.
- **NuGet Ready**: Easily available via public NuGet as a CLI tool.
