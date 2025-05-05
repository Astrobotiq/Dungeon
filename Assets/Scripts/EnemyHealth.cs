using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : IHealth
{
    
    [SerializeField] 
    private GameObject enemyPopupHealthCanvas;
    
    public override void TakeDamage(int damage, bool willPush)
    {
        if (TryGetComponent<SpiderEnemyBrain>(out var spider))
        {
            spider.DestroyWeb();
        }

        if (currentHealth - damage <= 0)
        {
            
            EventManager.Instance.InvokeOnEnemyKilled();
        }
        else if (GetComponent<MMPositionShaker>() is var shaker)
        {
            FeelManager.Instance.ShakeGameObject(shaker);
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
}