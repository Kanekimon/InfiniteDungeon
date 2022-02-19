using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public float health;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health < 0)
            Destroy(this.gameObject);
    }


    public void ChangeHealth(float value)
    {
        health += value;
    }
}
