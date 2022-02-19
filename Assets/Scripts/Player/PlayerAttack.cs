using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float Velocity;
    public float LifeTime;

    public float AttacksPerSecond;

    public GameObject projectile;

    public float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 1f/AttacksPerSecond)
        {
            if (Input.GetMouseButton(0))
            {
                Attack();
                timer = 0;
            }

        }
        timer += Time.deltaTime;
    }

    public void Attack()
    {
        GameObject g = ProjectilePoolManager.Instance.GetProjectileFromPool();
        if (g == null)
            return;
        g.SetActive(true);
        g.transform.parent = null;
        Vector2 dir = this.GetComponent<PlayerMovement>().GetDirection();

        g.transform.position = this.transform.position + new Vector3(dir.x * this.transform.localScale.x, dir.y * this.transform.localScale.y, 0);

        Projectile p = g.GetComponent<Projectile>();
        p.velocity = Velocity;
        p.lifeTime = LifeTime;
        p.direction = this.GetComponent<PlayerMovement>().GetDirection();
        p.fly = true;
    }
}
