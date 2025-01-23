using System;
using UnityEngine;

public class PlayerHealth : IHealth
{

    public event Action OnDeath;
    public event Action OnHeal;
    
    public override void TakeDamage(int damage)
    {        
        base.TakeDamage(damage);
        
        if (currentHealth <= 0)
        {
            OnDeath?.Invoke();
        }
    }

    public override void Heal(int heal)
    {
        base.Heal(heal);

        OnHeal?.Invoke();
    }
}