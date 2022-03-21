using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    public float AttacksPerSecond;
    public float Velocity;
    public Vector2 Direction;

    public AttackMode AttackMode;
    public ProjectileType projectileType;
    public float AttackRange;

    public float BaseDamage;

    public abstract void AttackAction(Vector2 targetCoords);

}

