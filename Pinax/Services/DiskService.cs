using Pinax.Models;

namespace Pinax.Services;

public static class DiskService
{
    public static List<Solution> GetSolutions(string rootDirectory)
    {
        if (!Directory.Exists(rootDirectory))
        {
            Console.WriteLine($"Directory '{rootDirectory}' does not exist");
        }

        List<Solution> solutions = new List<Solution>();

        var solutionFiles =
            Directory.GetFiles(rootDirectory, "*.sln", SearchOption.AllDirectories);

        foreach (string solutionFile in solutionFiles)
        {
            FileInfo solutionFileInfo = new FileInfo(solutionFile);

            var solution = new Solution
            {
                Name = solutionFile
            };

            var projectFiles =
                Directory.GetFiles(solutionFileInfo.DirectoryName, "*.csproj", SearchOption.AllDirectories);

            foreach (var projectFile in projectFiles)
            {
                var project = 
                    ProjectParser.GetProject(projectFile, File.ReadAllLines(projectFile));

                solution.Projects.Add(project);
            }

            solutions.Add(solution);
        }

        return solutions;
    }
}