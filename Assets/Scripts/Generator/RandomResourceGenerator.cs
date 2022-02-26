using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class RandomResourceGenerator
{



    public static List<Vector2> GenerateResources(Room r, List<Vector2> path)
    {
        List<Vector2> res = new List<Vector2>();


        Boundary b = r.GetBoundary();


        int max = 200;

        int added = 0;


        while(added < max)
        {
            int randomX = UnityEngine.Random.Range(b.startX + 1, b.endX);
            int randomY = UnityEngine.Random.Range(b.startY + 1, b.endY);

            Vector2 point = new Vector2(randomX, randomY);

            if (path.Contains(point))
                continue;

            res.Add(point);
            added++;
        }

        return res;
    }


}

