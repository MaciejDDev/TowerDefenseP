using UnityEngine;


public abstract class BaseStats : ScriptableObject
{

    public int MaxHealth;
    public float AttackRange;
    public int AttackDamage;
    public float AttackCooldown;
    public LayerMask TargetLayer;
}
