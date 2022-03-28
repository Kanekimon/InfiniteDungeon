using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "LootTable")]
public class LootTable : ScriptableObject
{
    [SerializeField]
    List<Loot> loot = new List<Loot>();

    public List<Loot> GetLoot() { return loot; }
}

[Serializable]
public class Loot
{
    public string Name;
    [Range(0f, 1f)]
    public float BaseProbability;
    public int Min;
    public int Max;
}

