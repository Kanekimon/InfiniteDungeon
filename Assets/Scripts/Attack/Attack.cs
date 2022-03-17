using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float AttacksPerSecond;
    public float Velocity;
    public bool canShot = true;
    public float timer;
    public Vector2 Direction;
    public List<string> Targets;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        AttacksPerSecond = this.GetComponent<AttributeSystem>().GetAttributeValue("dex");

        if (timer > 1f / AttacksPerSecond)
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


    public void Shot(Vector2 targetCoords)
    {
        if (canShot)
        {
            GameObject g = ProjectilePoolManager.Instance.GetProjectileFromPool();

            if (g == null)
                return;
            g.SetActive(true);
            g.transform.parent = null;


            Direction = (targetCoords - (Vector2)this.transform.position).normalized;

            g.transform.position = this.transform.position + new Vector3(Direction.x * this.transform.localScale.x, Direction.y * this.transform.localScale.y, 0);
            Projectile p = g.GetComponent<Projectile>();
            p.SetOwner(this.gameObject);
            p.velocity = 1;
            p.lifeTime = 3;
            p.direction = Direction;
            p.fly = true;
        }
    }

    private void OnDrawGizmos()
    {
        if(Direction != null)
        {
            Gizmos.DrawLine(transform.position, (Vector2) transform.position + Direction * 5f);
        }
    }

}
