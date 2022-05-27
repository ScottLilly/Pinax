using Pinax.Models;
using Pinax.Models.Projects;

namespace Pinax.Services;

public static class DiskService
{
    public static List<Solution> GetSolutions(string rootDirectory, DotNetVersions latestVersions)
    {
        if (!Directory.Exists(rootDirectory))
        {
            Console.WriteLine($"Directory '{rootDirectory}' does not exist");
        }

        var solutions = new List<Solution>();

        var solutionFiles =
            Directory.GetFiles(rootDirectory, "*.sln", SearchOption.AllDirectories);

        foreach (string solutionFile in solutionFiles)
        {
            var solutionFileInfo = new FileInfo(solutionFile);

            var solution = new Solution
            {
                Name = solutionFile
            };

            var projectFiles =
                Directory.GetFiles(solutionFileInfo.DirectoryName,
                    "*.csproj", SearchOption.AllDirectories);

            foreach (var projectFile in projectFiles)
            {
                DotNetProject project = 
                    ProjectParser.GetProject(projectFile, 
                        File.ReadAllLines(projectFile), latestVersions);

                solution.Projects.Add(project);
            }

            solutions.Add(solution);
        }

        return solutions;
    }
}