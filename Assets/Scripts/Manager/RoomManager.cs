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

    int[] maskX = { 0, 1, 0, -1 };
    int[] maskY = { -1, 0, 1, 0 };


    public int xSize;
    public int ySize;

    public List<Room> roomMap = new List<Room>();
    public Room currentRoom;
    public List<Room> activeRooms = new List<Room>();
    public List<Room> activeDebug = new List<Room>();

    public bool LoadFromData;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }


    public Room InitialRoom()
    {
        if (LoadFromData)
            GenerateFromSaveState();
        else
            MoveToNextRoom(Direction.none);


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


        ActivateCorrectRooms(currentRoom);
        currentRoom.depth = index.magnitude;

        GameManager.Instance.SetPlayerPos(currentRoom.center);
    }


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

    public void RemoveFromActive(Room r)
    {
        Destroy(r.GetParent());
        r.SetParent(null);
        activeRooms.Remove(r);
    }


    public void ActivateCorrectRooms(Room org)
    {
        if (!activeRooms.Contains(org))
            activeRooms.Add(org);

        int xInd = (int)org.index.x;
        int yInd = (int)org.index.y;

        List<Room> nRooms = new List<Room>();


        for(int i = 0; i < 4; i++)
        {
            int nx = xInd + maskX[i];
            int ny = yInd + maskY[i];

            Vector2 ne = new Vector2(nx, ny);
            nRooms.Add(GetRoomFromIndex(ne));
        }

        RemoveNotActiveRooms(nRooms);

        foreach(Room room in nRooms)
        {
            AddIfNotContaining(room);
        }
    }

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

    public Room GetRoomFromIndex(Vector2 ind)
    {
        return roomMap.Where(a => a.index == ind).FirstOrDefault();
    }

    public void ToggleRooms(bool act)
    {
        if (act)
        {
            foreach (Room room in roomMap)
            {
                if (!activeRooms.Any(a => a == room))
                {
                    if(room.GetTiles().Count >0)
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


    public void GenerateFromSaveState()
    {
        string fileUrl = Path.Combine(saveUrl, "savedRoom.txt");
        StreamReader sr = new StreamReader(fileUrl);
        roomMap = JsonConvert.DeserializeObject<List<Room>>(sr.ReadToEnd());
        currentRoom = roomMap.First();
        activeRooms.Add(RoomGeneratorManager.ReGenerateRoomFromSave(currentRoom));
        GameManager.Instance.SetPlayerPos(currentRoom.center);
    }

    public void SerializeRoom()
    {
        var settings = new Newtonsoft.Json.JsonSerializerSettings();
        settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        foreach (Room r in roomMap)
        {
            r.GenerateTileDataString();
        }

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

