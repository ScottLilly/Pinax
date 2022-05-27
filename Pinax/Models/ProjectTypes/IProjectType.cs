namespace Pinax.Models.ProjectTypes;

public interface IProjectType<out T>
{
    public T Type { get; }
    public Version Version { get; }
}