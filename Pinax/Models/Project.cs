namespace Pinax.Models;

public class Project
{
    public enum DotNetVersion
    {
        Unknown,
        Framework_1_0,
        Framework_1_1,
        Framework_2_0,
        Framework_3_0,
        Framework_3_5,
        Framework_4_0,
        Framework_4_5,
        Framework_4_5_1,
        Framework_4_5_2,
        Framework_4_6,
        Framework_4_6_1,
        Framework_4_6_2,
        Framework_4_7,
        Framework_4_7_1,
        Framework_4_7_2,
        Framework_4_8,
        Core_1_0,
        Core_1_1,
        Core_2_0,
        Core_2_1,
        Core_2_2,
        Core_3_0,
        Core_3_1,
        Net_5,
        Net_6,
        Net_7
    }

    public string FileName { get; set; }
    public string ShortName =>
        string.Join('\\', FileName.Split('\\').Skip(2));

    public List<ProjectType> ProjectTypes { get; set; } =
        new List<ProjectType>();
    public List<Package> Packages { get; } =
        new List<Package>();
}