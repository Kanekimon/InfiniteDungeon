using System.Collections.Generic;
using UnityEngine;

public static class RandomResourceGenerator
{

    static int[] maskX = { 0, 1, 1, 1, 0, -1, -1, -1 };
    static int[] maskY = { -1, -1, 0, 1, 1, 1, 0, 1 };

    public static List<Vector2> GenerateResources(Room r, List<Vector2> path)
    {
        List<Vector2> res = new List<Vector2>();

        int max = 200;

        int added = 0;


        while (added < max)
        {
            Vector2 point = GetRandomPointInRoom(r);

            if (path.Contains(point))
                continue;

            res.Add(point);
            added++;
        }

        return res;
    }



    public static List<Vector2> GenerateStone(Room r, List<Vector2> path, int seedPoints, float decayRate)
    {
        List<Vector2> stonePos = new List<Vector2>();


        for (int i = 0; i < seedPoints; i++)
        {
            Vector2 point = GetRandomPointInRoom(r);

            while (path.Contains(point))
                point = GetRandomPointInRoom(r);

            stonePos.Add(point);

            for (int j = 0; j < 8; j++)
            {
                Vector2 n = point + new Vector2(maskX[j], maskY[j]);
                if (!path.Contains(n))
                    stonePos.Add(n);
            }


        }


        return stonePos;
    }


    public static List<Resource> GenerateStone(Room r, List<Vector2> path, int seedPoints)
    {
        List<Resource> stonePos = new List<Resource>();


        for (int i = 0; i < seedPoints; i++)
        {
            Vector2 point = GetRandomPointInRoom(r);

            while (path.Contains(point))
                point = GetRandomPointInRoom(r);

            stonePos.Add(new Resource(point, "stone"));

            for (int j = 0; j < 8; j++)
            {
                Vector2 n = point + new Vector2(maskX[j], maskY[j]);
                if (!path.Contains(n) && r.bounds.IsInsideWalls(n))
                    stonePos.Add(new Resource(n,"stone"));
            }


        }


        return stonePos;
    }




    /// <summary>
    /// Adds a resource with given name, to the tile
    /// </summary>
    /// <param name="room">Room in which the tile is</param>
    /// <param name="parent">Parent tile gameobject</param>
    /// <param name="resource">Name of the resource</param>
    /// <returns></returns>
    public static GameObject GenerateResource(Room room, GameObject parent, Resource res)
    {
        GameObject resObj = GameObject.Instantiate(Resources.Load<GameObject>($"tiles/resources/{res.ResourceName}"));
        res.ResourceObject = resObj;
        room.AddResource(res);
        resObj.transform.position = parent.transform.position;
        resObj.transform.parent = parent.transform;
        return resObj;
    }




    static Vector2 GetRandomPointInRoom(Room r)
    {
        Boundary b = r.GetBoundary();

        int randomX = UnityEngine.Random.Range(b.startX + 1, b.endX-1);
        int randomY = UnityEngine.Random.Range(b.startY + 1, b.endY-1);


        Vector2 point = new Vector2(randomX, randomY);

        return point;
    }

}

