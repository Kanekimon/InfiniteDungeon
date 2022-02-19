using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float velocity;
    public float speed;
    public Vector2 direction;
    public float lifeTime;
    public float timer;
    public float damage;

    public bool fly = false;

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (fly)
        //{
        //    if (timer <= lifeTime)
        //    {

        //        speed = velocity * Time.deltaTime;
        //        this.transform.position += direction * speed;

        //        timer += Time.deltaTime;
        //    }
        //    else
        //    {
        //        Destroy(this.gameObject);
        //    }
        //}
    }

    private void FixedUpdate()
    {
        if (fly)
        {
            speed += velocity * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + direction * speed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject hit = collision.gameObject;

        if(hit.tag != "Wall")
        {
            HealthComponent health = null;
            hit.TryGetComponent<HealthComponent>(out health); 
            if(health != null)
            {
                health.ChangeHealth(-damage);
            }
        }

        ProjectilePoolManager.Instance.AddToPool(this.gameObject);

    }

}
