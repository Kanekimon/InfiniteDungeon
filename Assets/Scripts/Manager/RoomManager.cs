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

    public Dictionary<Room, Vector2> roomMap = new Dictionary<Room, Vector2>();

    public Room currentRoom;
    public List<Room> activeRooms = new List<Room> ();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }


    public Room InitialRoom()
    {
        Room r = RoomGeneratorManager.GenerateRoom(null, Direction.west, xSize, ySize);
        roomMap.Add(r, new Vector2(0, 0));
        currentRoom = r;
        return r;
    }

    public void MoveToNextRoom(Direction d)
    {
        Vector2 index = roomMap[currentRoom];
        index = ChangeIndex(d, index);

        if (!roomMap.ContainsValue(index))
        {
            Room r = RoomGeneratorManager.GenerateRoom(currentRoom, d, xSize, ySize);
            roomMap.Add(r, index);
            currentRoom = r;
            NPCManager.Instance.SpawnEnemies(currentRoom);
        }
        else
        {
            currentRoom = roomMap.Where(a => a.Value == index).First().Key;
        }

        currentRoom.depth = index.magnitude;

        Debug.Log($"Depth of current room: {currentRoom.depth}");

        GameManager.Instance.SetPlayerPos(currentRoom.center);


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

        if(!File.Exists(fileUrl))
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

