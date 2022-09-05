namespace Pinax.Engine.Services.FileReader;

public static class FileReaderFactory
{
    public static IFileReader GetDiskFileReader(string path)
    {
        return new DiskFileReader(path);
    }

    public static IFileReader GetGitFileReader(string token, string path)
    {
        return new GitFileReader(token, path);
    }
}