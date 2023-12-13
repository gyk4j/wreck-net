[![.github/workflows/dotnet-desktop.yml](https://github.com/gyk4j/wreck-net/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/gyk4j/wreck-net/actions/workflows/dotnet-desktop.yml)

Current experiment with [GitHub Actions](https://github.com/features/actions) using [MSBuild](https://github.com/dotnet/msbuild) on 
[legacy .NET Framework](https://dotnet.microsoft.com/en-us/learn/dotnet/what-is-dotnet-framework) project has not succeeded yet.

Possible cause of failing builds:
- Newer MS Build is unable to read or parse older Solution .sln format
- Not setting the right path to the checked-out Solution directory and Solution .sln file on the GitHub runner

CI Builds are expected to fail until a solution is found.

# A simplified C#.NET implementation of WRECK.

Not a direct port, but a rewrite.

Differences include:

- Different GUI
- Java-based Apache Tika library for metadata extraction is *NOT* integrated to reduce bloat
- 7-Zip is also *NOT INCLUDED* to parse through large archive and disc image files 
- A complete redesign based on .NET Framework API with no regard for maintaining resemblence to original Java version
 
