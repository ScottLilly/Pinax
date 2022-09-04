# Pinax

Console utility to report if C# projects have outdated versions of .NET (Standard, Framework, Core, or the new .NET) or if any NuGet packages are outdated. 
It will also report if there are any projects (.csproj files) under a solution directory, but not included in the solution.

This currently runs by pointing at a directory or directories.
It's designed to also work by pointing to a GitHub repository; however, GitHub throttles requests.
To make this program useful for GitHub, it needs to be converted to a GitHub App.

If you know how to setup a GitHub App, please contact me - as I'd like to have this run against repos.


## How to use
To run this, start the program and type:   
**NOTES:**
- When passing a drive path, Pinax searchs that directory and all diretories underneath it (recursively)
- Parameters can be combined and are not required to be in any specific order
- Pinax considers these to be the current versions of .NET: [LatestDotNetVersions.json](https://github.com/ScottLilly/Pinax/blob/master/Pinax.Console/LatestDotNetVersions.json)

## Command examples
See available parameters   
```--help```

Find solutions and projects under the e:\MyPublicProjects directory  
```--source:disk --location:e:\MyPublicProjects```

Find solutions and projects under the e:\MyPublicProjects and e:\MyPrivateProjects directories   
```--source:disk --location:e:\MyPublicProjects --location:e:\MyPrivateProjects```

Find solutions and projects under the e:\MyPublicProjects, excluding anything in (or under) e:\MyPublicProjects\ActivityTracker   
```--source:disk --location:e:\MyPublicProjects --exclude:e:\MyPublicProjects\ActivityTracker```

Find solutions and projects under the e:\MyPublicProjects, only showing proejcts that have outdated versions of .NET or NuGet packages   
```--source:disk --location:e:\MyPublicProjects --outdated```

Find solutions and projects under the e:\MyPublicProjects directory. Output results to ScanResults.json  
```--source:disk --location:e:\MyPublicProjects --output:e:\Output\ScanResults.json```

Find solutions and projects under the e:\MyPublicProjects directory. Do not report is a project is not used in a solution.  
```--source:disk --location:e:\MyPublicProjects --ignoreunused```

Find solutions and projects under the e:\MyPublicProjects directory.   
Use "build" level of version to warn if package is out-of-date   
Warning options: major, minor, build, revision (not case-sensitive)   
```--source:disk --location:e:\MyPublicProjects --warning:build```

## Sample output
"?" Indicates project is not included in solution file   
"*" Indicates version is out-of-date   
```
SOLUTION: e:\MyPublicProjects\SudokuSolver\SudokuSolver.sln
        PROJECT: SudokuEngine.csproj [.NET Framework 4.8.0.0]
?*      PROJECT: SudokuSolver.csproj [.NET Framework 4.5.0.0]
        PROJECT: TestSudokuEngine.csproj [.NET Framework 4.8.0.0]
SOLUTION: e:\MyPublicProjects\DigitsOfPi\DigitsOfPi.sln
*       PROJECT: DigitsOfPi.csproj [.NET Framework 4.7.2.0]
                PACKAGE: CommonServiceLocator [In project: 2.0.4] [Latest: 2.0.6.0]
*               PACKAGE: Prism.Core [In project: 7.2.0.1367] [Latest: 8.1.97.0]
*               PACKAGE: Prism.Wpf [In project: 7.2.0.1367] [Latest: 8.1.97.0]
                PACKAGE: System.ValueTuple [In project: 4.5.0] [Latest: 4.5.0.0]
*       PROJECT: DigitsOfPi.Engine.csproj [.NET Framework 4.7.2.0]
                PACKAGE: CommonServiceLocator [In project: 2.0.4] [Latest: 2.0.6.0]
*               PACKAGE: Prism.Core [In project: 7.2.0.1367] [Latest: 8.1.97.0]
*               PACKAGE: Prism.Wpf [In project: 7.2.0.1367] [Latest: 8.1.97.0]
                PACKAGE: System.ValueTuple [In project: 4.5.0] [Latest: 4.5.0.0]
```
