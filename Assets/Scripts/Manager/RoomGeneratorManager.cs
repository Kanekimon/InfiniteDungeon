using Assets.Scripts.Enum;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class RoomGeneratorManager : MonoBehaviour
{
    public static RoomGeneratorManager Instance;


    public int roomXSize;
    public int roomYSize;

    public GameObject WallPrefab;
    public GameObject FloorPrefab;

    public Dictionary<Vector2, Room> roomMap = new Dictionary<Vector2, Room>();
    public Dictionary<TileType, GameObject> tileResources = new Dictionary<TileType, GameObject>();


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        tileResources.Add(TileType.wall, WallPrefab);
        tileResources.Add(TileType.floor, FloorPrefab);

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector2 GenerateRoom()
    {
        Vector2 roomCoord = GetRoomCoord();

        Room r = new Room(System.Guid.NewGuid().ToString(), roomXSize, roomYSize);

        GameObject g = new GameObject();
        g.name = r.Id;

        for (int y = 0; y < roomYSize; y++)
        {
            for (int x = 0; x < roomXSize; x++)
            {
                TileType t = TileType.floor;
                if ((y == 0 || y == roomYSize - 1) || x == 0 || x == roomXSize - 1)
                {
                    t = TileType.wall;
                }
                r.AddTileToRoom(new Vector2(x, y), GenerateTile(g, x + (int) roomCoord.x, y + (int)roomCoord.y, t));

            }
        }
        roomMap.Add(roomCoord, r);
        return new Vector2((roomCoord.x + roomXSize) / 2, (roomCoord.y + roomYSize) / 2);
    }

    private Vector2 GetRoomCoord()
    {
        if (roomMap.Count == 0)
        {
            return Vector2.zero;
        }
        else
        {
            return GetRandom();
        }
    }


    private Vector2 GetRandom()
    {
        bool found = false;
        int counter = 0;
        while (!found)
        {

            if (counter >= 100)
                found = true;

            Vector2 randElement = roomMap.ElementAt(UnityEngine.Random.Range(0, roomMap.Count)).Key;
            Room r = roomMap[randElement];
            if (roomMap.ContainsKey(new Vector2(randElement.x + r.xLength, randElement.y)))
            {
                if (!roomMap.ContainsKey(new Vector2(randElement.x, randElement.y + r.yLength)))
                {
                    return new Vector2(randElement.x, randElement.y + r.yLength);
                }
            }
            else
            {
                return new Vector2(randElement.x + r.xLength, randElement.y);
            }
            counter++;
        }
        return new Vector2(-1, -1);
    }



    GameObject GenerateTile(GameObject parentRoom, int x, int y, TileType tileType)
    {
        GameObject w = Instantiate(tileResources[tileType]);
        w.transform.position = new Vector3(x, y,2);
        w.transform.parent = parentRoom.transform;
        return w;
    }
}
