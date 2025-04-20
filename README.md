# Celeste

* A (very messy) Celeste Decompilation! 
* To find the steam related stuff in the Celeste.cs file search for the "STEAM SHIT!" comment and you'll find it!
* You NEED assets from the original game to run this decompilation.

# Tools used

I used dotpeek to export the project and dnSpy to reference the scripts while fixing them, I also used [this](https://github.com/maybekoi/FSNSFix) to turn "namespace Celeste;" to "namespace Celeste {".

## How to build

Install the following dependencies:
- [.NET Framework 4.5.2](https://dotnet.microsoft.com/en-us/download/dotnet-framework/thank-you/net452-developer-pack-offline-installer)
- [Microsoft XNA Framework Redistributable 4.0](https://www.microsoft.com/en-us/download/details.aspx?id=20914)

Then clone the project, open the Visual Studio solution file, and you should be good to go!

## How to run

The only thing left to do after building is to copy the Content folder from a release of the game to the build folder (bin/x86/(Debug|Release)). **This project doesn't aim at giving the game for free as you need the assets which aren't included here.**