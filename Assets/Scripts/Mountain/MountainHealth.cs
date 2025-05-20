using UnityEngine;

public class MountainHealth : IHealth
{
    [SerializeField] GameObject particle;
    public override void TakeDamage(int damage, bool willPush)
    {
        
        if (currentHealth - damage <= 0)
        {
            EventManager.Instance.InvokeOnMountainDestroyed();
            var particleGO = Instantiate(this.particle, transform, false);
            Destroy(particleGO,1f);
        }
        base.TakeDamage(damage);
    }
}
