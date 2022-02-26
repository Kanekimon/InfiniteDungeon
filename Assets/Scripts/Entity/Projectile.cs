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
    public GameObject owner;

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

    public void SetOwner(GameObject g)
    {
        owner = g;

        damage = g.GetComponent<AttributeSystem>().GetAttributeValue("str");



    }



    private void FixedUpdate()
    {
        if (fly)
        {
            speed += velocity * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + direction + owner.GetComponent<Rigidbody2D>().velocity * speed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject hit = collision.gameObject;

        if(hit.tag != "Wall")
        {
            AttributeSystem att = null;
            hit.TryGetComponent(out att); 
            if(att != null)
            {
                att.ChangeHealth(-damage);
            }
        }

        this.owner = null;
        ProjectilePoolManager.Instance.AddToPool(this.gameObject);

    }

}
