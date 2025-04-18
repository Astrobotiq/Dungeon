using System;
using MoreMountains.Feedbacks;
using UnityEngine;

public class PlayerHealth : IHealth
{
    
    public override void TakeDamage(int damage)
    {
        EventManager.Instance.InvokeOnPlayerTakeDamage();
        
        if (TryGetComponent<MMPositionShaker>(out var shaker))
        {
            FeelManager.Instance.ShakeGameObject(shaker);
        }
        
        base.TakeDamage(damage);
    }
}