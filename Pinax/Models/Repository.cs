using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinax.Models;

public class Repository
{
    public string Name { get; set; }
    public List<Solution> Solutions { get; } =
        new List<Solution>();
}