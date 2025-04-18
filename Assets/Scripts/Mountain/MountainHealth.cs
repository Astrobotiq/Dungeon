using UnityEngine;

public class MountainHealth : IHealth
{
    public override void TakeDamage(int damage)
    {
        EventManager.Instance.InvokeOnMountainDestroyed();
        
        base.TakeDamage(damage);
    }
}
