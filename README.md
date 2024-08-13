[![.github/workflows/dotnet-desktop.yml](https://github.com/gyk4j/wreck-net/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/gyk4j/wreck-net/actions/workflows/dotnet-desktop.yml)

# A simplified C#.NET implementation of WRECK.

Not a direct port, but a rewrite of [WRECK][wreck], a 
Java tool for restoring file timestamps using embedded metadata, and heuristic 
rules based on file naming convention, personal habits and other methods of 
estimating a more accurate timestamp.

It is an essential tool when doing file backup and preservation to ensure that 
file creation, modification and access timestamps are as accurate as possible. 

Differences include:

1. Windows Form GUI, not Java Swing
2. Exclusion of Java-based Apache Tika library for metadata extraction to reduce
  bloat
3. A complete redesign based on [.NET Framework][dotnetfx] API and 
  [Windows Forms][winforms]
4. Charting of the statistics (given that the UI is not the same)
5. No statistics tracking and reporting
6. Missing file attributes backup, restoration and verification function.
  
![WRECK.NET GUI](../../wiki/assets/images/wreck-gui.png)
![WRECK>NET CLI](../../wiki/assets/images/wreck-cli.png)

For more information, see the [Wiki](../../wiki/Home).
  
## Introduction

WRECK.NET is a C#.NET desktop utility tool to *restore the file system timestamp* 
*attributes of files using the recorded metadata* if possible and rules / 
heuristics as a backup method on a *best-effort basis*. (See 
[Caveats](../../wiki/Caveats) for more details on the assumptions made).

The name WRECK is an acronym for **Walk, Retrieve, Extract, Correct and Keep**.

* Walk a given starting path or a single file
* Retrieve each file or directory encountered
* Extract metadata from it
* Correct the file system timestamps (creation, last modified, last accessed)
  using embedded metadata. In the absence of any useful metadata, it fallbacks 
  to using rules, heuristics and guesswork for estimating an approximate 
  appropriate timestamp on a best-effort basis (Refer to [Caveats](../../wiki/Caveats)).
* Keep the files for archival and preservation

It essentially processes files and directories recursively, extracting embedded 
metadata from common file formats (e.g. Microsoft Office documents, JPEG 
photos, MP4 videos, MP3 audio) using a few libraries to maximize success in 
metadata extraction, work out the appropriate timestamps, correct them in the
file system, and finally leaving the files ready for archival and preservation.

### Future plan and direction

In fact, I have been working on the .NET version and finding it more efficient 
and lightweight than the Java version. In all likelihood, I will continue to 
focus more on the .NET version going into the future.

The original Java WRECK may become stagnant with no updates or new features. 

[wreck]: https://github.com/gyk4j/wreck
[dotnetfx]: https://en.wikipedia.org/wiki/.NET_Framework  
[winforms]: https://en.wikipedia.org/wiki/Windows_Forms
