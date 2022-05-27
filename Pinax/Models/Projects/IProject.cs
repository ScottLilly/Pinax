namespace Pinax.Models.Projects;

public interface IProject<T>
{
    string ShortName { get; }

    List<T> ProjectTypes { get; }
    List<Package> Packages { get; }
}