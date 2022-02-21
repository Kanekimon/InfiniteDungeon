using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Attribute : MonoBehaviour
{
    public abstract string Name { get; set; }
    public abstract void ChangeValue(float value);

    public GameObject owner;

    public abstract float GetValue();

}

