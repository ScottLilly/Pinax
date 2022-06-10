namespace Pinax.Models;

public class FileDetails
{
    public string Path { get; }
    public string Name { get; }

    public FileDetails(string path, string name)
    {
        Path = path;
        Name = name;
    }
}