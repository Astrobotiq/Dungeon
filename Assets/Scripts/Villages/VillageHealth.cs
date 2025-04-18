using System;
using UnityEngine;

public class VillageHealth : IHealth
{
    public override void TakeDamage(int damage)
    {
        VillageManager.Instance.ChangeVillageHP(-1);
        EventManager.Instance.InvokeOnVillageTakeDamage();
        base.TakeDamage(damage);
    }
}
