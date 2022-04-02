using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IContextController
{
    public float[] UpdateInterestMap(Vector2 entityPos, int numberOfRays, Vector2[] directions, BaseLogic logic);
}

