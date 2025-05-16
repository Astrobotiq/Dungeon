using System;
using MoreMountains.Feedbacks;
using UnityEngine;

public class PlayerHealth : IHealth
{
    
    public override void TakeDamage(int damage, bool willPush)
    {
        EventManager.Instance.InvokeOnPlayerTakeDamage();
        
        if (TryGetComponent<MMPositionShaker>(out var shaker))
        {
            FeelManager.Instance.ShakeGameObject(shaker);
        }

        if (currentHealth - damage <= 0)
        {
            PlayerManager.Instance?.Unsubscribe(GetComponent<Player>());
        }
        
        base.TakeDamage(damage);
        InGameUITextMesh.Instance.UpdateSpecificPlayer(this.gameObject);
        
        InGameUITextMesh.Instance.UpdatePlayerBars();
    }
}