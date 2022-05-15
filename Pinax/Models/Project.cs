using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinax.Models;

public class Project
{
    public string FileName { get; set; }
    public List<Package> Packages { get; } =
        new List<Package>();
}