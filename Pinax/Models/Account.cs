using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinax.Models;

public class Account
{
    public string Name { get; set; }

    public List<Repository> Repositories { get; } =
        new List<Repository>();
}