using UnityEngine;

public abstract class IDamagable : MonoBehaviour
{
    //Maybe we can take damage from here. Maybe with enum DamageType Electricity, Fire, Ice, Smoke or something
    public abstract void Damage(int damage);
}
