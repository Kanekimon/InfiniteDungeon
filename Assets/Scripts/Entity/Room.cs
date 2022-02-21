using Assets.Scripts.Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room
{
    public string Id;

   // public Dictionary<Vector2, GameObject> RoomMap = new Dictionary<Vector2, GameObject>();

    private List<Tile> RoomTiles = new List<Tile>();

    public int xLength;
    public int yLength;

    public Vector2 index;
    
    public Boundary bounds;

    public float depth;

    public Vector2 center;

    public Room(string Id, Vector2 index,int xLen, int yLen, int startX, int startY)
    {
        this.Id = Id;
        this.xLength = xLen;
        this.yLength = yLen;
        this.index = index;

        bounds = new Boundary(startX, startY, startX+xLen, startY+yLen);

        center = new Vector2((startX+ bounds.endX)/2, (startY+ bounds.endY)/2);

        depth = Vector2.Distance(Vector2.zero, center);
    }

    public void AddTileToRoom(Vector2 coords, GameObject g, TileType t)
    {
        Tile tile = new Tile(coords.x, coords.y, t);
        RoomTiles.Add(tile);

        //RoomMap.Add(coords, g);

        if (t == TileType.door)
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
        }
    }

    private float ClampVals(float value, float[] clm)
    {
        int smallest = -1;
        for(int i = 0; i < clm.Length; i++)
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

    public Boundary GetBoundary()
    {
        return bounds;
    }
}
