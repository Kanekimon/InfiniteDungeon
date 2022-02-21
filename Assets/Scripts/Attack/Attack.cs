using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float AttacksPerSecond;
    public float Velocity;

    public bool canShot = true;
    public float timer;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(timer > 1f / AttacksPerSecond)
        {
            canShot = true;
            timer = 0;
        }
        else
        {
            canShot = false;
        }
        timer += Time.deltaTime;
    }


    public void Shot(Vector2 pDir)
    {
        if (canShot)
        {
            GameObject g = ProjectilePoolManager.Instance.GetProjectileFromPool();

            if (g == null)
                return;
            g.SetActive(true);
            g.transform.parent = null;


            Vector2 dir = (pDir - (Vector2)this.transform.position.normalized).normalized;

            g.transform.position = this.transform.position + new Vector3(dir.x * this.transform.localScale.x, dir.y * this.transform.localScale.y, 0);

            Projectile p = g.GetComponent<Projectile>();
            p.velocity = 1;
            p.lifeTime = 3;
            p.direction = dir;
            p.fly = true;
        }
    }

}
