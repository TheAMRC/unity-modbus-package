# IMG General - Modbus Package for Unity

A tool to simplify managing modbus connections in Unity and extends the functionality of [NModbus](https://github.com/NModbus/NModbus) (provided in the package as a plugin). Provides several helpful classes:

* ModbusConnection.cs - Holds a modbus connection to a device over TCP
* PollRegister.cs - Polls a register continuously and triggers an event when the register changes value.
* RegisterUtils.cs - Helpful util functions for working with bit registers.

## Getting Started

### Prerequisites

*What needs to be installed on your system before you install this repo. E.g.*

* [Unity](https://unity3d.com/) - The engine and build framework used
* [Visual Studio](https://visualstudio.microsoft.com/) - Code editor or equivelant

### Installing

#### Local Installation

1. Clone this repository to your local machine.
2. Follow the steps outlined in [Install a UPM package from a local folder](https://docs.unity3d.com/Manual/upm-ui-local.html)

#### Git Installation
1. Copy the link to the com.amrc.unitymodbus subfolder https://amrcgithub.shef.ac.uk/IMG/unity-modbus-package.git?path=/com.amrc.unitymodbus
2. Follow the steps outlined in [Install a UPM package from a Git URL](https://docs.unity3d.com/Manual/upm-ui-giturl.html)

## Deployment

The NModbus library used by this package should be supported on UWP devices (such as Hololens2) but this has yet to be tested.

### Coding Style

Coding style adheres to AMRC coding practices.

| Language | Standard |
| -- | -- |
| C# | [Microsoft .NET](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/) |

## Versioning

This project is using [SemVer](http://semver.org/) for versioning.

## Authors

* **Seth Roberts** - *Developer* - me1seth
