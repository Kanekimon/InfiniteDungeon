using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ContextSteeringData
{
    public float[] Interest;
    public float[] Danger;

    public void SetInterests(float[] inter)
    {
        Interest = inter;
    }

    public void SetDanger(float[] dang)
    {
        Danger = dang;
    }

}


