using System;
using UnityEngine;

public class NoDamageDrum : IMission
{
    void OnEnable()
    {
        isCompleted = true;
        EventManager.onDrumTakeDamage += UpdateOnMission;
    }

    void OnDisable()
    {
        EventManager.onDrumTakeDamage -= UpdateOnMission;
    }

    public override void UpdateOnMission()
    {
        isCompleted = false;
    }
}
