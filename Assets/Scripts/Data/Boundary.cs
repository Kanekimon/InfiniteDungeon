using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class Boundary
{

    public int startX;
    public int startY;
    public int endX;
    public int endY;


    public Boundary(int sX, int sY, int eX, int eY)
    {
        startX = sX;
        startY = sY;
        endX = eX;
        endY = eY;
    }

    public bool IsInBounds(Vector2 point)
    {
        bool inBounds = true;

        int pX = (int)point.x;
        int pY = (int)point.y;

        if (pX < startX || pX > endX)
            inBounds = false;
        if (pY < startY || pY > endY)
            inBounds = false;

        return inBounds;
    }




}

