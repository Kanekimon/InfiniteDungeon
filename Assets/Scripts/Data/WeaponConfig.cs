using UnityEngine;


[CreateAssetMenu(menuName = ("WeaponConfig"))]
public class WeaponConfig : ScriptableObject
{
    [SerializeField] AnimationClip attackAnimation;

    // add public getters i.g. 
    public AnimationClip GetAttackAnimation()
    {
        return attackAnimation;
    }
}
