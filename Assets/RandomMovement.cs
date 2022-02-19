using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{

    public float Speed = 10f;

    public float timer;
    public float delay = 3f;
    public Vector2 direction;
    public Vector3 target;
    public bool shooting = false;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    { 
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!shooting)
        {
            if (timer > delay)
            {
                delay = Random.Range(0.1f, 2f);
                float x = Random.Range(-1f, 1f);
                float y = Random.Range(-1f, 1f);
                direction = new Vector2(x, y);
                timer = 0;

            }
            rb.MovePosition(rb.position + direction * Speed * Time.fixedDeltaTime);

            timer += Time.fixedDeltaTime;
        }
    }
}
