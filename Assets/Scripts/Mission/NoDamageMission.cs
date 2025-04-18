using System;
using UnityEngine;

public class NoDamageMission : IMission
{
    void OnEnable()
    {
        isCompleted = true;
        EventManager.onPlayerTakeDamage += UpdateOnMission;
    }

    void OnDisable()
    {
        EventManager.onPlayerTakeDamage -= UpdateOnMission;
    }

    public override void UpdateOnMission()
    {
        isCompleted = false;
        Debug.Log("Player Hasar aldı \n" +
                  $"isCompleted : {isCompleted}");
        //Burada UI bilgisi gönderilir
    }
}