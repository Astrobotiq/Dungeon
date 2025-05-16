using System;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : Singleton<MissionManager>
{
    [SerializeField]
    private IMission[] missions;

    [SerializeField] 
    private List<IMission> ActiveMissions;

    public int GetCompletedMissionNumber()
    {
        int number = 0;
        foreach (var mission in ActiveMissions)
        {
            Debug.Log($"{mission.MissionInfo} completed ? " +
                      $"{mission.IsCompleted}");
            if (mission.IsCompleted)
            {
                number++;
            }
        }
        
        ActiveMissions.Clear();
        
        return number;
    }

    void Start()
    {
        missions = GetComponents<IMission>();
        ActiveMissions = new();
        DisableOpenMissions();
    }
    
    

    public void StartMission(MissionParameter mission)
    {
        
        switch (mission)
        {
            case MissionParameter.NoDamage:
                GetComponent<NoDamageMission>().enabled = true;
                ActiveMissions.Add(GetComponent<NoDamageMission>());
                break;
            case MissionParameter.KillFourEnemy:
                GetComponent<KillFourEnemyMission>().enabled = true;
                ActiveMissions.Add(GetComponent<KillFourEnemyMission>());
                break;
            case MissionParameter.NoDamageToVillages:
                GetComponent<NoDamageToVillageMission>().enabled = true;
                var vilMission = GetComponent<NoDamageToVillageMission>();
                ActiveMissions.Add(vilMission);
                break;
            case MissionParameter.DestroyAMountain:
                GetComponent<DestroyAMountainMission>().enabled = true;
                ActiveMissions.Add(GetComponent<DestroyAMountainMission>());
                break;
            case MissionParameter.PreventTwoEnemySpanws:
                GetComponent<PreventTwoSpawnerMission>().enabled = true;
                ActiveMissions.Add(GetComponent<PreventTwoSpawnerMission>());
                break;
            case MissionParameter.NoDamageToDrum:
                GetComponent<NoDamageDrum>().enabled = true;
                ActiveMissions.Add(GetComponent<NoDamageDrum>());
                break;
        }
    }

    public List<string> GetMissionInfo(List<MissionParameter> missionParameters)
    {
        List<string> missionInfos = new();

        foreach (var parameter in missionParameters)
        {
            switch (parameter)
            {
                case MissionParameter.NoDamage:
                    missionInfos.Add(GetComponent<NoDamageMission>().MissionInfo);
                    break;
                case MissionParameter.KillFourEnemy:
                    missionInfos.Add(GetComponent<KillFourEnemyMission>().MissionInfo);
                    break;
                case MissionParameter.NoDamageToVillages:
                    missionInfos.Add(GetComponent<NoDamageToVillageMission>().MissionInfo);
                    break;
                case MissionParameter.DestroyAMountain:
                    missionInfos.Add(GetComponent<DestroyAMountainMission>().MissionInfo);
                    break;
                case MissionParameter.PreventTwoEnemySpanws:
                    missionInfos.Add(GetComponent<PreventTwoSpawnerMission>().MissionInfo);
                    break;
                case MissionParameter.NoDamageToDrum:
                    missionInfos.Add(GetComponent<NoDamageDrum>().MissionInfo);
                    break;
            }
        }

        return missionInfos;
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
     NoDamageToDrum
}