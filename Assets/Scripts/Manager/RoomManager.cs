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
    public static string saveUrl = @"D:\Unity Workspace\InfiniteDungeon\Assets\SaveData";

    public static RoomManager Instance;

    public int xSize;
    public int ySize;

    //public Dictionary<Room, Vector2> roomMap = new Dictionary<Room, Vector2>();
    public List<Room> roomMap = new List<Room>();
    public Room currentRoom;
    public List<Room> activeRooms = new List<Room>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }


    public Room InitialRoom()
    {
        MoveToNextRoom(Direction.none);

        //Room r = RoomGeneratorManager.GenerateRoom(null, Vector2.zero, Direction.west, xSize, ySize);
        //roomMap.Add(r, new Vector2(0, 0));
        //roomMap.Add(r);
        //currentRoom = r;
        return currentRoom;
    }

    public void MoveToNextRoom(Direction d)
    {
        Vector2 index = Vector2.zero;
        if (currentRoom != null)
        {
            index = ChangeIndex(d, currentRoom.index);
        }

        currentRoom = GenerateRoom(index, d);


        activeRooms.Clear();
        activeRooms.Add(currentRoom);
        GetNeighbours();
        currentRoom.depth = index.magnitude;

        Debug.Log($"Depth of current room: {currentRoom.depth}");

        GameManager.Instance.SetPlayerPos(currentRoom.center);
    }


    public Room GenerateRoom(Vector2 index, Direction d)
    {
        Room r = null;
        if (!roomMap.Any(a => a.index == index))
        {
            r = RoomGeneratorManager.GenerateRoom(currentRoom, index, d, xSize, ySize);
            roomMap.Add(r);

            //NPCManager.Instance.SpawnEnemies(currentRoom);
        }
        else
        {
            r = roomMap.Where(a => a.index == index).First();
        }
        return r;
    }


    public void GetNeighbours()
    {

        Vector2 n = currentRoom.index;
        n.y += 1;
        activeRooms.Add(GenerateRoom(n, Direction.north));

        Vector2 e = currentRoom.index;
        e.x += 1;
        activeRooms.Add(GenerateRoom(e, Direction.east));


        Vector2 s = currentRoom.index;
        s.y -= 1;
        activeRooms.Add(GenerateRoom(s, Direction.south));

        Vector2 w = currentRoom.index;
        w.x -= 1;
        activeRooms.Add(GenerateRoom(w, Direction.west));

    }


    public void GenerateFromRoomData(Room r)
    {

    }

    public void SerializeRoom()
    {
        var settings = new Newtonsoft.Json.JsonSerializerSettings();
        settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

        string json = JsonConvert.SerializeObject(roomMap, settings);

        string fileUrl = Path.Combine(saveUrl, "savedRoom.txt");

        if (!File.Exists(fileUrl))
            File.Create(fileUrl);

        using (StreamWriter sw = new StreamWriter(fileUrl))
        {
            sw.Write(json);
        }
    }




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

