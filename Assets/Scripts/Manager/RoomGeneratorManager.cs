using Assets.Scripts.Data.StaticData;
using Assets.Scripts.Enum;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomGeneratorManager : MonoBehaviour
{
    public static RoomGeneratorManager Instance;

    public GameObject WallPrefab;
    public GameObject FloorPrefab;
    public GameObject DoorPrefab;
    public GameObject PathPrefab;
    public GameObject ResourcePrefab;


    public static Dictionary<TileType, GameObject> tileResources = new Dictionary<TileType, GameObject>();

    static int[] maskX = { 0, 1, 1, 1, 0, -1, -1, -1 };
    static int[] maskY = { -1, -1, 0, 1, 1, 1, 0, 1 };

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        if (tileResources.Count <= 0)
        {
            tileResources.Add(TileType.wall, WallPrefab);
            tileResources.Add(TileType.floor, FloorPrefab);
            tileResources.Add(TileType.door, DoorPrefab);
            tileResources.Add(TileType.path, PathPrefab);
            tileResources.Add(TileType.resource, ResourcePrefab);
        }

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public static Room ReGenerateRoom(Room r, Direction d = Direction.none)
    {
        if (r.GetTiles().Count == 0)
            r = ReGenerateRoomFromSave(r);
        else
        {

            GameObject g = new GameObject(r.Id);

            foreach (Tile t in r.GetTiles())
            {
                GenerateTile(g, t.x, t.y, t.type, r);
            }
            //if (d != Direction.none)
            //    r.playerSpawnPoint = GetRoomStartPoint(d, r);

            r.SetParent(g);
        }
        return r;
    }

    public static Room ReGenerateRoomFromSave(Room r)
    {
        GameObject g = new GameObject(r.Id);

        int x = 0;
        int y = 0;
        string tD = r.GetTileData();
        Debug.Log($"TD Length: {tD.Length}");
        for (int i = 0; i < tD.Length; i++)
        {
            if (x >= r.xLength)
            {
                x = 0;
                y++;
            }

            int index = (x) + (y * (r.xLength));
            GenerateTile(g, x + r.bounds.startX, y + r.bounds.startY, (TileType)int.Parse(tD[index].ToString()), r);
            x++;
        }

        r.SetParent(g);
        return r;
    }

    static Biome GetBiome(Room r, Direction d)
    {

        BiomeMatrix bMatrix = new BiomeMatrix();

        //Debug.Log($"Get biome for room ({index.x} | {index.y})");

        //Biome biome = Biome.grassland;
        //List<Biome> nBiomes = new List<Biome>();
        //for (int i = 0; i < 8; i++)
        //{
        //    Vector2 nIndex = index + new Vector2(maskX[i], maskY[i]);
        //    Room nR = RoomManager.Instance.GetRoomFromIndex(nIndex);
        //    if (nR != null)
        //    {
        //        nBiomes.Add(nR.biome);
        //    }

        //}



        List<Biome> pos = bMatrix.GetPossibleBiomeDirection(r, d);//bMatrix.GetPossibleBiomes(nBiomes);

        if (pos.Count == 0)
            return Biome.grassland;

        foreach (Biome b in pos)
        {
            Debug.Log($"Possible biome: {b}");
        }


        return pos.ElementAt(Random.Range(0, pos.Count));
    }

    public static Room GenerateRoom(Room oldRoom, Vector2 index, Direction d, int xSize, int ySize, bool genRa)
    {
        Vector2 startPoint = new Vector2(index.x * xSize, index.y * ySize);

        GameObject g = new GameObject();


        Room r = new Room(System.Guid.NewGuid().ToString(), index, xSize, ySize, (int)startPoint.x, (int)startPoint.y, g);
        r.SetBiome(GetBiome(r, d));
        g.name = $"Room: ({index.x} | {index.y})";
        Boundary b = r.GetBoundary();

        Vector2 startPointPath = GetRoomStartPoint(d, r);

        r.playerSpawnPoint = startPointPath;

        List<Vector2> path = new List<Vector2>();
        List<Vector2> res = new List<Vector2>();
        if (genRa)
        {
            if (startPoint != startPointPath)
            {
                path = RandomPathGenerator.GenerateRandomPath(r, startPointPath, Vector2.zero, PathMode.random);

                res = RandomResourceGenerator.GenerateStone(r, path, Random.Range(1, 10), Random.Range(1f, 1f));
            }
        }
        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                TileType t = TileType.floor;
                if ((y == 0 || y == ySize - 1) || x == 0 || x == xSize - 1)
                {
                    t = TileType.wall;
                }

                if ((x == (xSize / 2) && y == 0) || (x == (xSize / 2) && y == ySize - 1) || (x == 0 && y == (ySize / 2)) || (x == xSize - 1 && y == (ySize / 2)))
                    t = TileType.door;

                int xCord = x + (int)startPoint.x;
                int yCord = y + (int)startPoint.y;

                if (path.Count > 0 && path.Contains(new Vector2(xCord, yCord)))
                {
                    t = TileType.path;
                    r.AddTileToRoom(new Vector2(xCord, yCord), GenerateTile(g, xCord, yCord, t, r), t);
                }
                else if (res.Count > 0 && res.Contains(new Vector2(xCord, yCord)))
                {
                    GameObject tile = GenerateTile(g, xCord, yCord, t, r);
                    Vector2 tileCoord = new Vector2(xCord, yCord);
                    r.AddTileToRoom(tileCoord, tile, t);
                    r.AddResourceToTile(tileCoord, GenerateResource(tile));
                    r.aMap[x, y] = true;
                }
                else
                {
                    r.aMap[x, y] = t != TileType.floor && t != TileType.path;
                    r.AddTileToRoom(new Vector2(xCord, yCord), GenerateTile(g, xCord, yCord, t, r), t);
                }



            }
        }

        if (RoomManager.Instance.SpawnCorruptionCore)
            r.CreateCorruptionCore();

        return r;
    }


    public static Vector2 GetRoomStartPoint(Direction d, Room r)
    {
        Vector2 startPointPath = new Vector2();
        Boundary b = r.GetBoundary();
        if (d == Direction.north)
        {
            startPointPath = new Vector2(r.center.x, b.startY + 1);
        }
        else if (d == Direction.east)
        {
            startPointPath = new Vector2(b.startX + 1, r.center.y);
        }
        else if (d == Direction.south)
        {
            startPointPath = new Vector2(r.center.x, b.startY + r.yLength - 2);
        }
        else if (d == Direction.west)
        {
            startPointPath = new Vector2(b.startX + r.xLength - 2, r.center.y);
        }

        return startPointPath;
    }

    static GameObject GenerateResource(GameObject parent)
    {
        GameObject re = Instantiate(tileResources[TileType.resource]);

        re.transform.position = parent.transform.position;

        re.transform.parent = parent.transform;
        return re;
    }

    static GameObject GenerateTile(GameObject parentRoom, float x, float y, TileType tileType, Room r)
    {
        GameObject w = Instantiate(tileResources[tileType]);
        w.transform.position = new Vector3(x, y, 2);
        w.transform.parent = parentRoom.transform;

        if (tileType == TileType.door)
        {
            r.SetupDoor(new Vector2(x, y), w);
        }
        if (tileType == TileType.floor)
            w.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Textures/Tiles/{r.biome}/base");
        if (tileType == TileType.path)
            w.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Textures/Tiles/{r.biome}/path");


        return w;
    }


    public GameObject LoadHub(int xSize, int ySize, bool genRa)
    {
        GameObject hub = new GameObject();

        if (GameObject.Find("Hub") != null)
        {
            return GameObject.Find("Hub");
        }
        else
        {
            
        }

        return hub;
    }
}
