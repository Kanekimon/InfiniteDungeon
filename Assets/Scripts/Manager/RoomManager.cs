using Assets.Scripts.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class RoomManager : MonoBehaviour
{


    public static RoomManager Instance;

    int[] maskX = { 0, 1, 0, -1 };
    int[] maskY = { -1, 0, 1, 0 };


    public int xSize;
    public int ySize;

    public List<Room> roomMap = new List<Room>();
    public Room currentRoom;
    public List<Room> activeRooms = new List<Room>();
    public List<Room> activeDebug = new List<Room>();

    //Singelton Approach
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
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

        activeRooms.Add(RoomGeneratorManager.ReGenerateRoomFromSave(currentRoom));
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


        currentRoom = GenerateRoom(index, d);


        ActivateCorrectRooms(currentRoom);
        currentRoom.depth = index.magnitude;

        GameManager.Instance.SetPlayerPos(currentRoom.center);
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
        if (!roomMap.Any(a => a.index == index))
        {
            r = RoomGeneratorManager.GenerateRoom(currentRoom, index, d, xSize, ySize);
            roomMap.Add(r);
        }
        else
        {
            r = roomMap.Where(a => a.index == index).First();
            if (!activeRooms.Contains(r))
                RoomGeneratorManager.ReGenerateRoom(r);
        }
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
            RoomGeneratorManager.ReGenerateRoom(r);
        }
    }

    /// <summary>
    /// Returns room from given index
    /// </summary>
    /// <param name="ind">Room index</param>
    /// <returns>Room at index or null</returns>
    public Room GetRoomFromIndex(Vector2 ind)
    {
        return roomMap.Where(a => a.index == ind).FirstOrDefault();
    }

    /// <summary>
    /// Shows/Hides all rooms
    /// </summary>
    /// <param name="act"></param>
    public void ToggleRooms(bool act)
    {
        if (act)
        {
            foreach (Room room in roomMap)
            {
                if (!activeRooms.Any(a => a == room))
                {
                    if (room.GetTiles().Count > 0)
                        activeDebug.Add(RoomGeneratorManager.ReGenerateRoom(room));
                    else
                        activeDebug.Add(RoomGeneratorManager.ReGenerateRoomFromSave(room));
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


}

