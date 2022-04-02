using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class FollowContext : IContextController
{
    public float[] UpdateInterestMap(Vector2 entityPos, int numberOfRays, Vector2[] directions, BaseLogic logic)
    {
        float[] InterestMap = new float[numberOfRays];
        for(int i = 0; i < numberOfRays; i++)
        {
            Vector2 rotated = directions[i] + entityPos;

            GameObject attractor = logic.ClosestAttractor();

            Vector2 direction = ((Vector2)attractor.transform.position - entityPos).normalized;

            float dot = Vector2.Dot((rotated - entityPos), direction);

            InterestMap[i] = Mathf.Clamp01(dot);// * (range - (Mathf.Clamp(Vector2.Distance(entityPos, (Vector2)attractor.transform.position), 0, range))));
        }

        return InterestMap;
    }
}
