using Pinax.Engine.Models;
using Pinax.Engine.Models.Projects;

namespace Pinax.Engine.Services.FileReader;

public interface IFileReader
{
    List<Solution> GetSolutions();
    List<DotNetProject> GetDotNetProjects();
}