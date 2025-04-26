using System;
using System.Collections.Generic;
using UnityEngine;

public class VillageManager : Singleton<VillageManager>
{
    private List<GameObject> villages = new();
    [SerializeField] private int TotalHp;
    [SerializeField] private int MaxTotalHp = 10;

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

    public void ChangeVillageHP(int amount)
    {
        TotalHp += amount;

        if (TotalHp <= 0)
        {
            //TODO öldük demektir
        }

        if (TotalHp > MaxTotalHp)
        {
            TotalHp = MaxTotalHp;
        }
        
        InGameUITextMesh.Instance.updatePublicBar();
    }

    public float getHealthPercentage()
    {
        float temp_1 = (float)TotalHp;
        float temp_2 = (float)MaxTotalHp;

        float temp_3 = Mathf.FloorToInt((temp_1 / temp_2) * 100);
        
        Debug.Log("health percentage " + temp_3);
        return temp_3; 

    }
}
