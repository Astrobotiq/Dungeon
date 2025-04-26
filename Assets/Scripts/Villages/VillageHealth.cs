using System;
using UnityEngine;

public class VillageHealth : IHealth
{
    [SerializeField] 
    private SoundManager soundManager;
    
    public float VillageTakeDamageSoundVolume = 1f;
    public override void TakeDamage(int damage)
    {
        VillageManager.Instance.ChangeVillageHP(-1);
        EventManager.Instance.InvokeOnVillageTakeDamage();
        base.TakeDamage(damage);

        PlayerManager.Instance.playerListForEnemyAI.Remove(gameObject);
        
        soundManager = GameObject.FindWithTag("SoundManager").GetComponent<SoundManager>();
        soundManager.PlaySound(SoundType.VillageTakeDamageSound, VillageTakeDamageSoundVolume);
    }
}
