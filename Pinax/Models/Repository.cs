﻿namespace Pinax.Models;

public class Repository
{
    public string Name { get; set; }
    public List<Solution> Solutions { get; } =
        new List<Solution>();
}