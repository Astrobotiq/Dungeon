using UnityEngine;

public class MountainHealth : IHealth
{
    public override void TakeDamage(int damage, bool willPush)
    {
        EventManager.Instance.InvokeOnMountainDestroyed();
        
        base.TakeDamage(damage);
    }
}
