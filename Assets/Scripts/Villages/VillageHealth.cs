using System;
using UnityEngine;

public class VillageHealth : IHealth
{
    [SerializeField] 
    private SoundManager soundManager;
    
    public float VillageTakeDamageSoundVolume = 1f;
    public override void TakeDamage(int damage, bool willPush)
    {
        VillageManager.Instance.ChangeVillageHP(-1);
        EventManager.Instance.InvokeOnVillageTakeDamage();
        
        soundManager = GameObject.FindWithTag("SoundManager").GetComponent<SoundManager>();
        soundManager.PlaySound(SoundType.VillageTakeDamageSound, VillageTakeDamageSoundVolume);
        
        if (currentHealth - damage <= 0)
        {
            VillageManager.Instance.UnSubscribe(this.gameObject);
            PlayerManager.Instance.playerListForEnemyAI.Remove(gameObject);
        }
        
        base.TakeDamage(damage);
        
    }
}
