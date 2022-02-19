using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public string Id;

    public Dictionary<Vector2, GameObject> RoomMap = new Dictionary<Vector2,GameObject>();

    public int xLength;
    public int yLength;


    public Room(string Id, int xLen, int yLen)
    {
        this.Id = Id;
        this.xLength = xLen;
        this.yLength = yLen;
    }

    public void AddTileToRoom(Vector2 coords, GameObject g)
    {
        RoomMap.Add(coords, g);
    }
}
