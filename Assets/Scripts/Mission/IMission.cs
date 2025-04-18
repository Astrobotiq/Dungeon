using UnityEngine;

public abstract class IMission: MonoBehaviour
{
    public RewardType RewardType;
    protected bool isCompleted;
    public bool IsCompleted => isCompleted;
    public abstract void UpdateOnMission();
}