using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthAttribute : Attribute
{
    public float health;

    private string AttributeName;

    public override string Name { get { return AttributeName; } set => AttributeName = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health < 0)
            Destroy(this.owner);
    }


    public override void ChangeValue(float value)
    {
        health += value;
    }

    public override float GetValue()
    {
        return health;
    }
}
