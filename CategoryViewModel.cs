using System;
using System.Collections.Generic;
using AutoNest.Data.Entities;


public class CategoryViewModel
{
    public string Name { get; set; }
    public string Description { get; set; }

    public IEnumerable<Part> Parts { get; set; }
}