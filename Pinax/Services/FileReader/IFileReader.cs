using Pinax.Models;

namespace Pinax.Services.FileReader;

public interface IFileReader
{
    IEnumerable<FileDetails> GetSolutionFiles();
}