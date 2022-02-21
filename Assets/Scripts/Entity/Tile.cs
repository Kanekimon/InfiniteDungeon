using Assets.Scripts.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class Tile
{
    public TileType type;
    public float x;
    public float y;

    public Tile(float x, float y, TileType pt)
    {
        this.x = x;
        this.y = y;
        this.type = pt;
    }

}

