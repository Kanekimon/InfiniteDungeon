using Assets.Scripts.Enum;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class RoomGeneratorManager : MonoBehaviour
{
    public static RoomGeneratorManager Instance;

    public GameObject WallPrefab;
    public GameObject FloorPrefab;
    public GameObject DoorPrefab;

    public static Dictionary<TileType, GameObject> tileResources = new Dictionary<TileType, GameObject>();


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        tileResources.Add(TileType.wall, WallPrefab);
        tileResources.Add(TileType.floor, FloorPrefab);
        tileResources.Add(TileType.door, DoorPrefab);

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public static Room ReGenerateRoom(Room r)
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

    public static Room GenerateRoom(Room oldRoom, Vector2 index, Direction d, int xSize, int ySize)
    {
        Vector2 startPoint = new Vector2();

        if (oldRoom != null)
        {
            Boundary oldBounds = oldRoom.GetBoundary();

            if (d == Direction.north)
                startPoint = new Vector2(oldBounds.startX, oldBounds.endY);
            else if (d == Direction.south)
                startPoint = new Vector2(oldBounds.startX, oldBounds.startY - ySize);
            else if (d == Direction.west)
                startPoint = new Vector2(oldBounds.startX - xSize, oldBounds.startY);
            else
                startPoint = new Vector2(oldBounds.endX, oldBounds.startY);
        }
        else
        {
            startPoint = new Vector2(0, 0);
        }

        GameObject g = new GameObject();
        Room r = new Room(System.Guid.NewGuid().ToString(), index, xSize, ySize, (int)startPoint.x, (int)startPoint.y, g);
        g.name = r.Id;

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

                r.AddTileToRoom(new Vector2(xCord, yCord), GenerateTile(g, xCord, yCord, t, r), t);

            }
        }

        return r;
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

        return w;
    }
}
