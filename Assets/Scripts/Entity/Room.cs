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

    public CorruptionCore core;

    public bool[,] aMap;

    private GameObject parent;
    private GameObject resource;

    List<Door> doors = new List<Door>();

    [JsonProperty]
    Dictionary<int,int> enemies = new Dictionary<int,int>();

    [JsonProperty]
    private string tileData = "";


    /// <summary>
    /// 
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="index"></param>
    /// <param name="xLen"></param>
    /// <param name="yLen"></param>
    /// <param name="startX"></param>
    /// <param name="startY"></param>
    /// <param name="par"></param>
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
        aMap = new bool[xLen, yLen];
    }


    /// <summary>
    /// Registers a tile to the tilemap
    /// </summary>
    /// <param name="coords"></param>
    /// <param name="g"></param>
    /// <param name="t"></param>
    public void AddTileToRoom(Vector2 coords, GameObject g, TileType t)
    {
        Tile tile = new Tile(coords.x, coords.y, t, g);
        RoomTiles.Add(tile);
    }

    /// <summary>
    /// Adds resource to the tile
    /// </summary>
    /// <param name="tileCoord"></param>
    /// <param name="resource"></param>
    public void AddResourceToTile(Vector2 tileCoord, GameObject resource)
    {
        resourceMap[RoomTiles.Where(a => a.x == (int)tileCoord.x && a.y == (int)tileCoord.y).First()] = resource;
    }

    /// <summary>
    /// Returns all enemies in this room
    /// Dictionary Key: Enemy Id
    /// Dictionary Value: Count of entities of id
    /// </summary>
    /// <returns></returns>
    public Dictionary<int,int> GetEnemies()
    {
        return enemies;
    }

    /// <summary>
    /// Returns the parent container of this room
    /// </summary>
    /// <returns></returns>
    public GameObject GetParent()
    {
        return parent;
    }

    /// <summary>
    /// Sets the parent container for this object
    /// </summary>
    /// <param name="parent"></param>
    public void SetParent(GameObject parent)
    {
        this.parent = parent;
    }

    /// <summary>
    /// Returns Tile map
    /// </summary>
    /// <returns></returns>
    public string GetTileData()
    {
        return tileData;
    }

    /// <summary>
    /// Returns the resources
    /// </summary>
    /// <returns></returns>
    public GameObject GetResources()
    {
        return resource;
    }

    /// <summary>
    /// Registers Enemy in room
    /// </summary>
    /// <param name="enemyId"></param>
    /// <param name="amount"></param>
    public void AddEnemyToRoom(int enemyId, int amount)
    {
        if (enemies.ContainsKey(enemyId))
            enemies[enemyId] += amount;
        else
            enemies.Add(enemyId, amount);
    }

    /// <summary>
    /// Creates doors
    /// </summary>
    /// <param name="coords"></param>
    /// <param name="g"></param>
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

    /// <summary>
    /// Clamps Values to Direction Vectors
    /// </summary>
    /// <param name="value"></param>
    /// <param name="clm"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Returns the Bounds of the room
    /// </summary>
    /// <returns></returns>
    public Boundary GetBoundary()
    {
        return bounds;
    }

    /// <summary>
    /// Returns a list of the Tiles
    /// </summary>
    /// <returns></returns>
    public List<Tile> GetTiles()
    {
        return RoomTiles;
    }

    /// <summary>
    /// Generates the string for saving the room
    /// </summary>
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

    /// <summary>
    /// Creates the corruption core on a random tile
    /// </summary>
    public void CreateCorruptionCore()
    {
        if(core == null)
        {
            Tile t = this.RoomTiles.ElementAt(UnityEngine.Random.Range(0,RoomTiles.Count));
            GameObject g = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Core"));
            g.transform.SetParent(t.tileObject.transform);
            g.transform.localPosition = Vector2.zero;

        }
    }
}
