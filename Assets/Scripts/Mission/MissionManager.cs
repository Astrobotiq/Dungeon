using System;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : Singleton<MissionManager>
{
    [SerializeField]
    private IMission[] missions;

    void Start()
    {
        missions = GetComponents<IMission>();
        
        DisableOpenMissions();
    }

    public void StartMission(MissionParameter mission)
    {
        switch (mission)
        {
            case MissionParameter.NoDamage:
                GetComponent<NoDamageMission>().enabled = true;
                break;
            case MissionParameter.KillFourEnemy:
                GetComponent<NoDamageMission>().enabled = true;
                break;
            case MissionParameter.NoDamageToVillages:
                GetComponent<NoDamageMission>().enabled = true;
                break;
            case MissionParameter.DestroyAMountain:
                GetComponent<NoDamageMission>().enabled = true;
                break;
            case MissionParameter.PreventTwoEnemySpanws:
                GetComponent<NoDamageMission>().enabled = true;
                break;
        }
    }

    public void DisableOpenMissions()
    {
        if (missions == null || missions.Length == 0)
            return;
        
        foreach (var mission in missions)
        {
            mission.enabled = false;
        }
    }
}

public enum MissionParameter
{
     NoDamage,
     KillFourEnemy,
     NoDamageToVillages,
     DestroyAMountain,
     PreventTwoEnemySpanws,
}