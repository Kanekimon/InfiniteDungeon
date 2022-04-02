using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class WeightedDirection : MonoBehaviour
{

    public GameObject attractor;



    public float delay;
    public float timer;

    public float speed;

    public bool SpawnAttractors;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (SpawnAttractors)
        {
            if (timer > delay)
            {
                GameObject at = GameObject.Instantiate(attractor);
                at.transform.position = new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
                timer = 0;
            }
        }
        timer += Time.deltaTime;

    }



}
