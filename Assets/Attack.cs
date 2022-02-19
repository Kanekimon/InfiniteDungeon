using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject target;


    public float AttacksPerSecond;


    public float timer;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, this.GetComponent<RandomMovement>().direction, 10, LayerMask.GetMask("Player"));
            if (hit.collider != null)
            {
                if (hit.collider.tag == "Player")
                {
                    this.GetComponent<RandomMovement>().shooting = true;
                    target = hit.collider.gameObject;
                }
            }
        }
        if (target != null)
        {
            if (timer > 1f / AttacksPerSecond)
            {

                Shot();
                timer = 0;


            }
            timer += Time.deltaTime;

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            collision.gameObject.GetComponent<HealthComponent>().ChangeHealth(-1);
        }
    }

    public void Shot()
    {
        GameObject g = ProjectilePoolManager.Instance.GetProjectileFromPool();
        if (g == null)
            return;
        g.SetActive(true);
        g.transform.parent = null;
        Vector2 dir = (Vector2)(target.transform.position - this.transform.position).normalized;

        g.transform.position = this.transform.position + new Vector3(dir.x * this.transform.localScale.x, dir.y * this.transform.localScale.y, 0);

        Projectile p = g.GetComponent<Projectile>();
        p.velocity = 1;
        p.lifeTime = 3;
        p.direction = dir;
        p.fly = true;
    }


    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Vector2 target = this.GetComponent<RandomMovement>().direction * 10f;
        Gizmos.DrawRay(this.transform.position, target);
    }
}
