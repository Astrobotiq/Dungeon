using System;
using MoreMountains.Feedbacks;

public class EnemyHealth : IHealth
{
    public event Action OnDeath;
    public event Action OnHeal;
    
    public override void TakeDamage(int damage)
    {
        if (TryGetComponent<SpiderEnemyBrain>(out var spider))
        {
            spider.DestroyWeb();
        }
        
        if (GetComponent<MMPositionShaker>() is var shaker)
        {
            FeelManager.Instance.ShakeGameObject(shaker);
        }

        if (currentHealth - damage <= 0)
        {
            
            EventManager.Instance.InvokeOnEnemyKilled();
        }
        
        base.TakeDamage(damage);
    }

    public override void Heal(int heal)
    {
        base.Heal(heal);
        
        OnHeal?.Invoke();
    }
}