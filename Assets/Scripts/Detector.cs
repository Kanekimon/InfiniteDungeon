using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Detector : MonoBehaviour
{

    public bool CheckIfDirectionIsFree(Vector2 currentPos, Vector2 dir)
    {
        Ray r = new Ray(currentPos, dir);
        RaycastHit2D hit = Physics2D.Raycast(currentPos, dir, dir.magnitude, LayerMask.GetMask(new string[]{ "Player", "Obstacle", "Wall", "Enemy"}));
        return hit.collider == null;
    }

}

