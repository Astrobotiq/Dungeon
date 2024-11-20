using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : Singleton<GridManager>
{
    [SerializeField] private GameObject GridPrefab;
    [SerializeField] public GameObject selectedGrid { set; get; }

    [SerializeField] public int StartPosition; 
    [SerializeField] public int EndPosition; 
    //ToDo sistemden tilemap kısmını çıkarıp kendi instatiate mantığımızı yerleştirmeliyiz.
    
    public List<List<GameObject>> GridList;

    void Start()
    {
        InstantiateGrids(StartPosition,EndPosition,GridPrefab);
    }

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
    
    public float GetCenter()
    {
        return (StartPosition + EndPosition) / 2;
    }

    public void SetSelectedGrid(GameObject Grid)
    {
        selectedGrid = Grid;
        PlayerManager.Instance.SetPlayerPosition(Grid);
    }

    void DeSelectGrid()
    {
        Debug.Log("Deselect");
        selectedGrid = null;
    }

    void InstantiateGrids(int start, int end, GameObject Grid)
    {
        for (int i = start; i < end; i++)
        {
            for (int j = start; j < end; j++)
            {
                var pos = new Vector3(i, 0, j);
                var GridGO = Instantiate(Grid, pos, Quaternion.identity);
                GridGO.transform.SetParent(this.transform);
                AddGrid(pos,GridGO);
            }
        }
    }

    public Grid getGridFromLocation(Vector3 input_vector3)
    {
        Grid temp = GridList[(int)input_vector3.x][(int)input_vector3.z].GetComponent<Grid>();
        return temp;
    }
}