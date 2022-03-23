using Assets.Scripts.Enum;
using Assets.Scripts.Generator;
using Assets.Scripts.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomManager : MonoBehaviour
{


    public static RoomManager Instance;

    int[] maskX = { 0, 1, 0, -1 };
    int[] maskY = { -1, 0, 1, 0 };


    public int xSize;
    public int ySize;
    public bool genRa;

    public float gizSphere;

    public List<Room> roomMap = new List<Room>();
    public Room currentRoom;
    public List<Room> activeRooms = new List<Room>();
    public List<Room> hubRooms = new List<Room>();
    public List<Room> activeDebug = new List<Room>();

    public GameObject Hub;
    public GameObject Run;

    public bool SpawnCorruptionCore;

    public bool ShowWalkableArea = false;

    //Singelton Approach
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }


    /// <summary>
    /// 
    /// </summary>
    public Room LoadHub(bool newGame)
    {
        Room hub = null;
        hub = LoadHubFromSave();



        return hub;
    }


    //public Room CreateHub()
    //{

    //}

    public Room LoadHubFromSave()
    {
        int saveId = StartParameters.newGame ? -1 : StartParameters.saveGame;
        Room r = null;

        string urlRoom = "";

        if (saveId == -1)
        {
            r = RoomGenerator.CreateFromPreset("main_hub");
            hubRooms.Add(r);
        }
        else
        {

        }


        return r;
    }


    /// <summary>
    /// Creates the first Room for a new game
    /// </summary>
    /// <returns></returns>
    public Room InitialRoom()
    {
        MoveToNextRoom(Direction.none);
        return currentRoom;
    }

    /// <summary>
    /// Loads the roomLayout and player position from given save
    /// </summary>
    /// <param name="rooms">all rooms from the save</param>
    /// <param name="playerIndex">Index of room where player was</param>
    /// <param name="playerPos">Actuall position of player</param>
    public void StartFromSave(List<Room> rooms, Vector2 playerIndex, Vector2 playerPos)
    {
        roomMap = rooms;

        currentRoom = roomMap.Where(r => r.index == playerIndex).FirstOrDefault();

        NPCManager.Instance.SpawnFromSave(currentRoom.GetEnemies(), currentRoom);

        activeRooms.Add(RoomGenerator.ReGenerateRoomFromSave(currentRoom));
        GameManager.Instance.SetPlayerPos(playerPos);
    }

    /// <summary>
    /// Triggers the creation of the next room
    /// </summary>
    /// <param name="d"></param>
    public void MoveToNextRoom(Direction d)
    {
        Vector2 index = Vector2.zero;
        if (currentRoom != null)
        {
            index = ChangeIndex(d, currentRoom.index);
        }


        Room oldTemp = currentRoom;
        currentRoom = GenerateRoom(index, d);

        ActivateCorrectRooms(currentRoom);
        UiManager.Instance.ChangeDepth(currentRoom);

        if (oldTemp.core != null)
            NPCManager.Instance.SetActiveStatusForRoom(oldTemp, true);
    }


    public void AddToRoomMap(Room r)
    {
        if (Hub.activeInHierarchy)
            hubRooms.Add(r);
        else
            roomMap.Add(r);
    }

    public List<Room> GetRoomMap()
    {
        if (Hub.activeInHierarchy)
            return hubRooms;
        else
            return roomMap;
    }

    /// <summary>
    /// Returns the current room
    /// </summary>
    /// <returns>Room in which Player is</returns>
    public Room GetCurrentRoom()
    {
        return currentRoom;
    }

    /// <summary>
    /// Triggers the static Method for generating new rooms
    /// or the regeneration of an inactive room
    /// </summary>
    /// <param name="index">index of room</param>
    /// <param name="d"></param>
    /// <returns></returns>
    public Room GenerateRoom(Vector2 index, Direction d)
    {
        Room r = null;
        List<Room> map = GetRoomMap();
        if (!map.Any(a => a.index == index))
        {
            r = RoomGenerator.GenerateRoom(index, d, xSize, ySize);
            AddToRoomMap(r);
        }
        else
        {
            r = map.Where(a => a.index == index).First();
            if (!activeRooms.Contains(r))
                RoomGenerator.ReGenerateRoom(r);

            if (r.core != null)
                NPCManager.Instance.SetActiveStatusForRoom(r, false);
        }

        r.playerSpawnPoint = RoomGenerator.GetRoomStartPoint(d, r);

        GameManager.Instance.SetPlayerPos(r.playerSpawnPoint);


        return r;
    }

    /// <summary>
    /// Destorys gameobjects of room which is
    /// not longer in proximity
    /// </summary>
    /// <param name="r"></param>
    public void RemoveFromActive(Room r)
    {
        Destroy(r.GetParent());
        r.SetParent(null);
        activeRooms.Remove(r);
    }

    /// <summary>
    /// Activates the room and neighbouring rooms
    /// </summary>
    /// <param name="org"></param>
    public void ActivateCorrectRooms(Room org)
    {
        if (!activeRooms.Contains(org))
            activeRooms.Add(org);

        int xInd = (int)org.index.x;
        int yInd = (int)org.index.y;

        List<Room> nRooms = new List<Room>();


        for (int i = 0; i < 4; i++)
        {
            int nx = xInd + maskX[i];
            int ny = yInd + maskY[i];

            Vector2 ne = new Vector2(nx, ny);
            nRooms.Add(GetRoomFromIndex(ne));
        }

        RemoveNotActiveRooms(nRooms);

        foreach (Room room in nRooms)
        {
            AddIfNotContaining(room);
        }
    }

    /// <summary>
    /// Removes all rooms that are not active anymore
    /// </summary>
    /// <param name="neighs"></param>
    void RemoveNotActiveRooms(List<Room> neighs)
    {
        for (int i = activeRooms.Count - 1; i >= 0; i--)
        {
            Room r = activeRooms[i];
            if (!neighs.Contains(r) && r != currentRoom)
            {
                RemoveFromActive(r);
            }
        }
    }

    /// <summary>
    /// Checks if a room is already inside the active list
    /// </summary>
    /// <param name="r"></param>
    void AddIfNotContaining(Room r)
    {
        if (r == null)
            return;

        if (!activeRooms.Any(a => a.index == r.index))
        {
            activeRooms.Add(r);
            RoomGenerator.ReGenerateRoom(r);
        }
    }

    /// <summary>
    /// Returns room from given index
    /// </summary>
    /// <param name="ind">Room index</param>
    /// <returns>Room at index or null</returns>
    public Room GetRoomFromIndex(Vector2 ind)
    {
        return GetRoomMap().Where(a => a.index == ind).FirstOrDefault();
    }

    /// <summary>
    /// Shows/Hides all rooms
    /// </summary>
    /// <param name="act"></param>
    public void ToggleRooms(bool act)
    {
        if (act)
        {
            foreach (Room room in GetRoomMap())
            {
                if (!activeRooms.Any(a => a == room))
                {
                    if (room.GetTiles().Count > 0)
                        activeDebug.Add(RoomGenerator.ReGenerateRoom(room));
                    else
                        activeDebug.Add(RoomGenerator.ReGenerateRoomFromSave(room));
                }
            }
        }
        else
        {
            foreach (Room r in activeDebug)
            {
                Destroy(r.GetParent());
                r.SetParent(null);
            }
        }
    }

    /// <summary>
    /// Returns wether a tile is walkable or not
    /// </summary>
    /// <param name="r"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public bool IsTileWalkable(Room r, int x, int y)
    {
        return !r.aMap[x - r.bounds.startX, y - r.bounds.startY];
    }

    /// <summary>
    /// Changes index based on direction
    /// </summary>
    /// <param name="d">Direction of next room</param>
    /// <param name="ind">index of old room</param>
    /// <returns></returns>
    Vector2 ChangeIndex(Direction d, Vector2 ind)
    {
        switch (d)
        {
            case Direction.north:
                ind.y += 1;
                break;
            case Direction.south:
                ind.y -= 1;
                break;
            case Direction.west:
                ind.x -= 1;
                break;
            case Direction.east:
                ind.x += 1;
                break;

        }

        return ind;
    }

    public void LeaveHub()
    {
        Hub.SetActive(false);

        currentRoom = InitialRoom();
        GameManager.Instance.SpawnPlayer(currentRoom.center);
    }

    public void ReturnToHub()
    {
        GetRoomMap().Clear();
        Destroy(Run);
        Run = new GameObject();
        Hub.SetActive(true);


        //GameManager.Instance.SpawnPlayer();
    }

    public GameObject GetMapParent()
    {
        if (Hub.activeSelf)
            return Hub;
        else
            return Run;
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            foreach (Room room in roomMap)
            {
                Boundary b = room.GetBoundary();

                Gizmos.color = Color.blue;
                Vector2 leftBottom = new Vector2(b.startX, b.startY);
                Gizmos.DrawSphere(leftBottom, gizSphere);

                Gizmos.color = Color.red;
                Vector2 leftTop = new Vector2(b.startX, b.endY);
                Gizmos.DrawSphere(leftTop, gizSphere);

                Gizmos.color = Color.green;
                Vector2 rightBottom = new Vector2(b.endX, b.startY);
                Gizmos.DrawSphere(rightBottom, gizSphere);

                Gizmos.color = Color.yellow;
                Vector2 topRight = new Vector2(b.endX, b.endY);
                Gizmos.DrawSphere(topRight, gizSphere);

                Gizmos.color = Color.black;
                Gizmos.DrawSphere(room.center, gizSphere);
            }

            if (ShowWalkableArea)
            {
                for (int y = currentRoom.bounds.startY; y < currentRoom.bounds.endY + 1; y++)
                {
                    for (int x = currentRoom.bounds.startX; x < currentRoom.bounds.endX + 1; x++)
                    {
                        if (currentRoom.aMap[x - currentRoom.bounds.startX, y - currentRoom.bounds.startY])
                            Gizmos.color = Color.green;
                        else
                            Gizmos.color = Color.red;

                        Gizmos.DrawCube(new Vector2(x, y), new Vector3(0.5f, 0.5f));
                    }
                }
            }
        }

    }

}
