using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinax.Models;

public class Solution
{
    public string Name { get; set; }

    public List<Project> Projects { get; } =
        new List<Project>();
}