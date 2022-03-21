using UnityEngine;

public class RangedAttack : Attack
{
    public override void AttackAction(Vector2 targetCoords)
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

