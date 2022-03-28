using System.Collections.Generic;
using UnityEngine;




public class AttackSystem : MonoBehaviour
{
    public float AttacksPerSecond;
    public float Velocity;
    public bool canShot = true;
    public float timer;
    public Vector2 Direction;
    public List<string> Targets;
    public AttackMode AttackMode;
    public GameObject Owner;
    public Attack attack;

    // Start is called before the first frame update
    void Start()
    {
        Owner = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        AttacksPerSecond = 1;//this.GetComponent<AttributeSystem>().GetAttributeValue("dex");

        if (!canShot)
        {
            if (timer > 1f / AttacksPerSecond)
            {
                canShot = true;
                timer = 0;
            }
        }
        timer += Time.deltaTime;
    }


    public void Attack(Vector2 targetCoords)
    {

        GameObject weapon = null;
        foreach (Transform t in this.transform)
        {
            if (t.gameObject.CompareTag("Weapon"))
                weapon = t.gameObject;
        }
        if (weapon != null && canShot)
        {
            canShot = false;
            weapon.GetComponent<Attack>().AttackAction(targetCoords);
        }

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
        if (Direction != null)
        {
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + Direction * 5f);
        }
    }

}
