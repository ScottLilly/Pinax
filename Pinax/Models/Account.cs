namespace Pinax.Models;

public class Account
{
    public string Name { get; set; }

    public List<Repository> Repositories { get; } =
        new List<Repository>();
}