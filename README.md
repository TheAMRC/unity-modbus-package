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

## Contributing

Please feel free to contribute anything you'd like. Just get in touch with me and I'll add you as an editor.

### Coding Style

Coding style adheres to AMRC coding practices.

| Language | Standard |
| -- | -- |
| C# | [Microsoft .NET](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/) |
| Javascript | [AirBnB](https://github.com/airbnb/javascript) |
| Java | [Google Java Style Guide](https://google.github.io/styleguide/javaguide.html) |
| Python | [PEP-8](https://www.python.org/dev/peps/pep-0008/) |
| C++ 17 | [ISOCPP](https://github.com/isocpp/CppCoreGuidelines) |

*Delete as appropiate for the project and where required state additional languages. 
E.g. specific database technologises used and the standard being followed.*

## Versioning

This project is using [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repositiory](https://github.com/your/project/tags).

## Authors

* **Seth Roberts** - *Developer* - [me1seth](http://amrcgithub.shef.ac.uk/me1seth)

## License

This project is funded under IMG General.

## Acknowledgments

*This markdown sheet is quite handy! [Link](https://github.com/adam-p/markdown-here/wiki/Markdown-Cheatsheet)*
