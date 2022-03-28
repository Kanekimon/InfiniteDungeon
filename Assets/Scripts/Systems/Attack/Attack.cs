using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    public float AttacksPerSecond;
    public float Velocity;
    public Vector2 Direction;

    public string anim_state;
    public WeaponConfig config;

    public AttackMode AttackMode;
    public ProjectileType projectileType;
    public float AttackRange;

    public float BaseDamage;
    public Animator animator;

    public abstract void AttackAction(Vector2 targetCoords);


}

