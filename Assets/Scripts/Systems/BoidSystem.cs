using System.Collections.Generic;
using UnityEngine;

public class BoidSystem : MonoBehaviour
{

    public List<GameObject> nearby = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject g = collision.gameObject;
        if (g.tag == "Enemy" && !nearby.Contains(g))
        {
            nearby.Add(g);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (nearby.Contains(collision.gameObject))
        {
            nearby.Remove(collision.gameObject);
        }
    }
}
