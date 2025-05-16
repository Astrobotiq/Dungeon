using UnityEngine;

public abstract class IMission: MonoBehaviour
{
    public string MissionInfo;
    public RewardType RewardType;
    
    [SerializeField]
    protected bool isCompleted;
    public bool IsCompleted => isCompleted;
    public abstract void UpdateOnMission();
}