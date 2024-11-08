using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : Singleton<GridManager>
{
    public static List<List<GameObject>> GridList;
    
    
    void Update()
    {
        
    }

    public void AddGrid(Vector3 pos, GameObject Grid)
    {
        if (GridList == null)
        {
            GridList = new();
        }
        int xPos = (int)Math.Floor(pos.x);
        int zPOs = (int)Math.Floor(pos.z);

        while (GridList.Count <= xPos)
        {
            GridList.Add(new List<GameObject>());
        }

        while (GridList[xPos].Count <= zPOs)
        {
            GridList[xPos].Add(null);
        }
        
        GridList[xPos][zPOs] = Grid;
    }

    public void RemoveGrid(Vector3 pos)
    {
        if (GridList == null)
        {
            return;
        }
        int xPos = (int)Math.Floor(pos.x);
        int zPOs = (int)Math.Floor(pos.z);
        GridList[xPos][zPOs] = null;
    }
}