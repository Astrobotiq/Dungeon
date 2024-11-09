using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : Singleton<GridManager>
{
    [SerializeField] public GameObject selectedGrid { set; get; }
    
    public List<List<GameObject>> GridList;
    
    void OnEnable()
    {
        InputManager.Instance.onRightClicked += DeSelectGrid;
    }

    void OnDisable()
    {
        InputManager.Instance.onRightClicked -= DeSelectGrid;
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

    public GameObject GetGrid()
    {
        if (GridList == null)
        {
            return null;
        }

        for (int i = 0; i < GridList.Count; i++)
        {
            List<GameObject> Grids = GridList[i];
            for (int j = 0; j < Grids.Count; j++)
            {
                GameObject Grid = Grids[j];
                if (Grid != null)
                {
                    return Grid;
                }
            }
        }
        return null;
    }

    public void SetSelectedGrid(GameObject Grid)
    {
        selectedGrid = Grid;
        PlayerManager.Instance.SetPlayerPosition(Grid);
    }

    void DeSelectGrid()
    {
        selectedGrid = null;
    }
}