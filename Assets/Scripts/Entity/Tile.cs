using Assets.Scripts.Enum;
using System;
using UnityEngine;

[Serializable]
public class Tile
{
    public TileType type;
    public float x;
    public float y;
    public GameObject tileObject;

    public Tile(float x, float y, TileType pt, GameObject tileOb)
    {
        this.x = x;
        this.y = y;
        this.type = pt;
        this.tileObject = tileOb;
    }

}

