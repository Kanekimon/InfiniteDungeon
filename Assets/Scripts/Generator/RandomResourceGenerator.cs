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

        for(int x = b.startX+1; x < b.endX ; x++)
        {
            for(int y = b.startY+1; y < b.endY ; y++)
            {
                Vector2 point = new Vector2(x, y);

                if (path.Contains(point))
                    continue;

                res.Add(point);

            }
        }



        return res;
    }


}

