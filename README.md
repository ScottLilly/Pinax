# Pinax

Console utility to report if C# projects have outdated versions of .NET (Framework, Core, or the new .NET) or if the NuGet packages are out-of-date. 
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
- Pinax considers the "current" version of the different .NETs to be:
  - .NET Framework: 4.8
  - .NET Core 3.1
  - .NET 6

### Command examples
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
