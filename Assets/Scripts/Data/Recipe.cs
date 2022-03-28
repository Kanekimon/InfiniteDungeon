using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Recipe
{
    public string ResultItemName  { get; set; }
    public int ResultAmount { get; set; }
    public List<RecipeMaterial> Materials { get; set; }
}

public class RecipeMaterial
{
    public Item Material;
    public int Amount;
}

