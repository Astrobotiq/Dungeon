using System;
using System.Collections.Generic;
using UnityEngine;

public class VillageManager : Singleton<VillageManager>
{
    private List<GameObject> villages = new();
    [SerializeField] private int TotalHp;
    [SerializeField] private int MaxTotalHp;

    public void Start()
    {
        TotalHp = MaxTotalHp;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Subscribe(GameObject village)
    {
        if (villages.Contains(village))
        {
            return;
        }
        villages.Add(village);
    }

    public void UnSubscribe(GameObject village)
    {
        if (!villages.Contains(village))
        {
            return;
        }

        villages.Remove(village);
    }

    public int getTotalHp() {
        return TotalHp;
    }
}
