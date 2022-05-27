using System.ComponentModel.DataAnnotations;

namespace Pinax;

public static class Enums
{
    public enum DotNetType
    {
        Unknown,
        [Display(Name = ".NET Standard")]
        Standard,
        [Display(Name = ".NET Framework")]
        Framework,
        [Display(Name = ".NET Core")]
        Core,
        [Display(Name = ".NET")]
        DotNet
    }

    public enum Source
    {
        Unknown,
        Disk,
        GitHub
    }

    public enum WarningLevel
    {
        None,
        Major,
        Minor,
        Build,
        Revision
    }
}