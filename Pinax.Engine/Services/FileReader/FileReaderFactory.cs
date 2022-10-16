namespace Pinax.Engine.Services.FileReader;

public static class FileReaderFactory
{
    public static IFileReader GetDiskFileReader(string path)
    {
        return new DiskFileReader(path);
    }
}