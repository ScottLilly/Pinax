using Pinax.Models;

namespace Pinax.Services.FileReader;

public class DiskFileReader : IFileReader
{
    private readonly string _path;

    public DiskFileReader(string path)
    {
        _path = path;
    }

    public IEnumerable<FileDetails> GetSolutionFiles()
    {
        return 
            Directory.GetFiles(_path, "*.sln", SearchOption.AllDirectories)
                .Select(f => new FileInfo(f))
                .Select(f => new FileDetails(f.DirectoryName ?? _path, f.Name));
    }
}