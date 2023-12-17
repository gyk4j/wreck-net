[![.github/workflows/dotnet-desktop.yml](https://github.com/gyk4j/wreck-net/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/gyk4j/wreck-net/actions/workflows/dotnet-desktop.yml)

> [!NOTE]
> Current experiment with [GitHub Actions](https://github.com/features/actions) using [MSBuild](https://github.com/dotnet/msbuild) on 
> [legacy .NET Framework](https://dotnet.microsoft.com/en-us/learn/dotnet/what-is-dotnet-framework) project has not succeeded yet.
>   
> Possible causes of failing builds:
> - Newer MS Build versions are unwilling/unable to read or parse older Solution `.sln` and Project `.csproj` format
>   i.e. Microsoft might have mandated users to upgrade their Solution/Project files to newer Visual Studio format
> - ~~Not setting the right path to the checked-out Solution directory and Solution .sln file on the GitHub runner~~
>
> Troubleshooting/checks done:
> - Actions runner's current working directory is in the repo's root and is able to locate the `Wreck.sln` solution file
> - The Solution is tested to build cleanly with **MSBuild** on **.NET Framework 3.5** locally on development workstation.
> 
> So conclusion is the Solution `.sln` and project `.csproj` files are not corrupt, but the modern **.NET Core** and
> **.NET Standard** tools are not working well with them. Maybe Microsoft has decided to drop backward compatibility.
>   
> CI Builds are expected to fail with the newer **MS Build** on **Windows Server 2019**.
> 
> No solution is foreseeable if Microsoft does not wish to support building legacy .NET Framework 3.5 desktop projects.

# A simplified C#.NET implementation of WRECK.

Not a direct port, but a rewrite.

Differences include:

- Different GUI
- Java-based Apache Tika library for metadata extraction is *NOT* integrated to reduce bloat
- ~~7-Zip is also *NOT INCLUDED* to parse through large archive and disc image files~~ (Correction: 7-Zip support re-added on 15 Dec 2023) 
- A complete redesign based on .NET Framework API with no regard for maintaining resemblence to original Java version
 
