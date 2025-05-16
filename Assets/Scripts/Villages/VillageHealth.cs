using System;
using UnityEngine;
using UnityEngine.UI;

public class VillageHealth : IHealth
{
    [SerializeField] 
    private SoundManager soundManager;
    
    public float VillageTakeDamageSoundVolume = 1f;
    
    public GameObject villagePopupHealthCanvas;
    
    public GameObject AttackPreviewHealthCanvas;
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
        
        if(villagePopupHealthCanvas==null)
        {
            Debug.Log("Village Health Canvas assign edilmemiştir");
            return;
        }
        
        Slider slider = villagePopupHealthCanvas.transform.GetChild(0).gameObject.GetComponent<Slider>();
        slider.value = GetComponent<VillageHealth>().getHealthPercentage();
        
    }
}
