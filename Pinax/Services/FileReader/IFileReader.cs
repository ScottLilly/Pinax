using Pinax.Models;
using Pinax.Models.Projects;

namespace Pinax.Services.FileReader;

public interface IFileReader
{
    List<Solution> GetSolutions();
    List<DotNetProject> GetDotNetProjects();
}