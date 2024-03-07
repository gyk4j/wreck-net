[![.github/workflows/dotnet-desktop.yml](https://github.com/gyk4j/wreck-net/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/gyk4j/wreck-net/actions/workflows/dotnet-desktop.yml)

# A simplified C#.NET implementation of WRECK.

Not a direct port, but a rewrite.

Differences include:

- Different GUI
- Java-based Apache Tika library for metadata extraction is *NOT* integrated to reduce bloat
- A complete redesign based on .NET Framework API 
  ~~with no regard for maintaining code resemblence to original Java version~~
  (See update below)
  
## Note on compatibility with Java-based [WRECK][wreck]

WRECK.NET started with the original stance that it would be a simplified total
re-implementation in .NET like a separate application without aiming for 
feature parity. This is to enable me to focus on rapid development of a working
.NET application with the core functionalities completed.

Much has changed since then. As the prototype Windows Forms application took 
shape and began working, I began to become ambitious in trying to make the two
code bases look similar. This is to enable me to update both applications in 
future. 

Since 10 Jan 2024, I have been incorporating as much reusable logic as
possible, to the point where I was doing a line-by-line porting for the core
classes and methods. In summary, most of the crucial backend and file 
processing logic have been ported over.

### Latest status

Update on 7 March 2024

- A `JShim` Java Runtime API emulation layer to mirror call references to 
  classes and method that wraps around the .NET API
- Majority of codes from the original [WRECK][wreck] has been ported over
  
What are still missing or different today?

1. Charting of the statistics (given that the UI is not the same)
2. More statistics tracking and reporting
3. Missing file attributes backup, restoration and verification function.

### Future plan and direction

In fact, I have been working on the .NET version and finding it more efficient 
and lightweight than the Java version. In all likelihood, I will continue to 
focus more on the .NET version going into the future.

The original Java WRECK may become stagnant with no updates or new features. 
  
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

![WRECK.NET GUI](../../wiki/assets/images/wreck-gui.png)
![WRECK>NET CLI](../../wiki/assets/images/wreck-cli.png)

For more information, see the [Wiki](../../wiki/Home).

[wreck]: https://github.com/gyk4j/wreck
 
