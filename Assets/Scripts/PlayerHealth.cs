using System;
using MoreMountains.Feedbacks;
using UnityEngine;

public class PlayerHealth : IHealth
{
    
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (TryGetComponent<MMPositionShaker>(out var shaker))
        {
            FeelManager.Instance.ShakeGameObject(shaker);
        }
    }
}