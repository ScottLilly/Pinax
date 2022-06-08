namespace Pinax.Models.Projects;

public interface IProject<T>
{
    string Path { get; }
    string Name { get; }

    List<T> ProjectTypes { get; }
    List<Package> Packages { get; }
}