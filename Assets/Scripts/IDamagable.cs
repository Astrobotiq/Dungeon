using System;
using UnityEngine;

public class IDamagable : MonoBehaviour
{
    [SerializeField] 
    private IHealth health;

    void Awake()
    {
        health = GetComponent<IHealth>();
    }

    //Maybe we can take damage from here. Maybe with enum DamageType Electricity, Fire, Ice, Smoke or something
    //Burayı yazdım ama unutmuşum. Kullanılabilirliğini tartışmak gerek.
    //Belki burayı farklı amaçlar için kullanırız diye tuttum. Konuşulsun silinir.
    public virtual void Damage(int damage, bool willPush = false)
    {
        //Buranın içinde başka şeyler yazılabilir.
        health.TakeDamage(damage);
    }
}