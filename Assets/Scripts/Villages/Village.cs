using System;
using UnityEngine;

public class Village : MonoBehaviour
{
    [SerializeField, Range(0,5)]
    private int rewardAmount;
    
    [SerializeField]
    private VillageType villageType;
    
    private Grid grid;

    public Grid GetGrid() => grid;
    
    public VillageType VillageType
    {
        get => villageType;
        private set => villageType = value;
    }

    public int GetRewardAmount => rewardAmount;

    void Start()
    {
        VillageManager.Instance.Subscribe(this.gameObject);
    }

    void OnDestroy()
    {
        VillageManager.Instance.UnSubscribe(this.gameObject);
    }

    public void SetGrid(Grid grid)
    {
        if (this.grid != null)
            this.grid.GridObject = null;

        this.grid = grid;
        this.grid.GridObject = gameObject;
    }
}

public enum VillageType
{
    Capital,
    Fishing,
    Farm,
    Agriculture
}
