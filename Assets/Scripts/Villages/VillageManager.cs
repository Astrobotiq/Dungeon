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
        decreaseTotalHP();
    }

    public int getTotalHp() {
        return TotalHp;
    }
    public float getHealthPercentage()
    {
        float temp_1 = (float)TotalHp;
        float temp_2 = (float)MaxTotalHp;

        float temp_3 = Mathf.FloorToInt((temp_1 / temp_2) * 100);
        
        //Debug.Log("health percentage " + temp_3);
        return temp_3; 

    }

    public void decreaseTotalHP()
    {
        TotalHp--;

        if (TotalHp <= 0)
        {
            TotalHp = 0;
        }
        else if(TotalHp >= MaxTotalHp)
        {
            TotalHp = MaxTotalHp;
        }
        
        InGameUITextMesh.Instance.updatePublicBar();
    }
}
