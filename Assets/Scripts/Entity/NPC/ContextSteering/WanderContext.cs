using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;


public class WanderContext : IContextController
{
    public float[] UpdateInterestMap(Vector2 entityPos, int numberOfRays, Vector2[] directions, BaseLogic logic)
    {
        float[] InterestMap = new float[numberOfRays];
        for (int i = 0; i < numberOfRays; i++)
        {
            Vector2 rotated = directions[i];
            float x = Random.Range(-2f, 2f);
            float y = Random.Range(-2f, 2f);
            Vector2 TargetPoint = new Vector2(entityPos.x + x, entityPos.y + y);
            Vector2 direction = (TargetPoint - entityPos).normalized;

            float dot = Vector2.Dot((rotated - entityPos).normalized, direction.normalized);

            InterestMap[i] = Mathf.Clamp01(dot);
        }

        return InterestMap;
    }
}
