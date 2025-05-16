using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : IHealth
{
    public event Action OnDeath;
    public event Action OnHeal;
    
    public GameObject enemyPopupHealthCanvas;

    public GameObject AttackPreviewHealthCanvas;
    
    public override void TakeDamage(int damage, bool willPush)
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
        
        if(enemyPopupHealthCanvas==null)
        {
            Debug.Log("Enemy Health Popup Canvas assign edilmemiştir");
            return;
        }
        
        Slider slider = enemyPopupHealthCanvas.transform.GetChild(0).gameObject.GetComponent<Slider>();
        slider.value = GetComponent<EnemyHealth>().getHealthPercentage();
    }

    public override void Heal(int heal)
    {
        base.Heal(heal);
        
        OnHeal?.Invoke();
    }
}