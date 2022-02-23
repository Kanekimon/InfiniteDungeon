using Assets.Scripts.Enum;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Room
{
    public string Id;

    // public Dictionary<Vector2, GameObject> RoomMap = new Dictionary<Vector2, GameObject>();

    private List<Tile> RoomTiles = new List<Tile>();
    private Dictionary<Tile, GameObject> resourceMap = new Dictionary<Tile, GameObject>();

    public int xLength;
    public int yLength;
    public float depth;
    public Vector2 index;
    public Vector2 center;
    public Vector2 playerSpawnPoint;
    public Boundary bounds;

    private GameObject parent;
    private GameObject resource;

    List<Door> doors = new List<Door>();

    [JsonProperty]
    Dictionary<int,int> enemies = new Dictionary<int,int>();

    [JsonProperty]
    private string tileData = "";



    public Room(string Id, Vector2 index, int xLen, int yLen, int startX, int startY, GameObject par)
    {
        this.Id = Id;
        this.xLength = xLen;
        this.yLength = yLen;
        this.index = index;
        this.parent = par;

        bounds = new Boundary(startX, startY, startX + xLen-1, startY + yLen-1);

        center = new Vector2((startX + bounds.endX) / 2, (startY + bounds.endY) / 2);

        depth = Vector2.Distance(Vector2.zero, center);
    }

    public void AddTileToRoom(Vector2 coords, GameObject g, TileType t)
    {
        Tile tile = new Tile(coords.x, coords.y, t);
        RoomTiles.Add(tile);
    }


    public void AddResourceToTile(Vector2 tileCoord, GameObject resource)
    {
        resourceMap[RoomTiles.Where(a => a.x == (int)tileCoord.x && a.y == (int)tileCoord.y).First()] = resource;
    }

    public Dictionary<int,int> GetEnemies()
    {
        return enemies;
    }

    public GameObject GetParent()
    {
        return parent;
    }

    public void SetParent(GameObject parent)
    {
        this.parent = parent;
    }

    public string GetTileData()
    {
        return tileData;
    }

    public void AddEnemyToRoom(int enemyId, int amount)
    {
        if (enemies.ContainsKey(enemyId))
            enemies[enemyId] += amount;
        else
            enemies.Add(enemyId, amount);
    }

    public void SetupDoor(Vector2 coords, GameObject g)
    {
        Vector2 fromCenter = (center - coords).normalized;
        fromCenter = new Vector2(ClampVals(fromCenter.x, new float[] { 0, 1, -1 }), ClampVals(fromCenter.y, new float[] { 0, 1, -1 }));

        Direction d = Direction.east;

        if (fromCenter == Vector2.down)
            d = Direction.north;
        else if (fromCenter == Vector2.up)
            d = Direction.south;
        else if (fromCenter == Vector2.right)
            d = Direction.west;

        g.GetComponent<Door>().SetDir(d);
        doors.Add(g.GetComponent<Door>());
    }

    private float ClampVals(float value, float[] clm)
    {
        int smallest = -1;
        for (int i = 0; i < clm.Length; i++)
        {
            if (smallest == -1)
                smallest = i;
            else
            {
                float cur = clm[i] - value;
                float old = clm[smallest] - value;

                if (Mathf.Abs(cur) < Mathf.Abs(old))
                    smallest = i;
            }
        }
        return clm[smallest];
    }

    public Boundary GetBoundary()
    {
        return bounds;
    }

    public List<Tile> GetTiles()
    {
        return RoomTiles;
    }

    public void GenerateTileDataString()
    {
        if (RoomTiles.Count > 0)
        {
            tileData = "";
            foreach (Tile tile in RoomTiles)
            {
                tileData += (int)tile.type + "";
            }
        }
    }
}
