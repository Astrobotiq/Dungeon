using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : Singleton<GridManager>
{
    [SerializeField] private GameObject GridPrefab;
    
    public GameObject selectedGrid = null;

    [SerializeField] public int StartPosition; 
    
    [SerializeField] public int EndPosition; 
    
    public List<List<GameObject>> GridList;

    public bool IsInSearchState = false;

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

    public void ResetTable()
    {
        foreach (var list in GridList)
        {
            foreach (var grid in list)
            {
                grid.GetComponent<Grid>().IsAvailable = false;
                grid.GetComponent<Grid>().MaterialController.SetMaterialDefault();
            }
        }
    }

    public void SetSelectedGridFromOutside(Vector3 pos, bool willTravel, int range = 0)
    {
        if (selectedGrid != null)
        {
            selectedGrid.GetComponent<Grid>().MaterialController.ResetOutlineScale();
        }
        selectedGrid = getGridFromLocation(pos).gameObject;
        selectedGrid.GetComponent<Grid>().MaterialController.SetOutlineScale();

        if (willTravel)
        {
            Algorithm algorithm = new Algorithm();
            HashSet<Vector3> set = algorithm.startAlgorithm(selectedGrid.GetComponent<Grid>(), range);
            Debug.Log(set.Count);
            foreach (var position in set)
            {
                var grid = getGridFromLocation(position);
                grid.MaterialController.SetMaterialWalkable();
                grid.IsAvailable = true;
            }
            IsInSearchState = true;
        }
    }

    public void SetSelectedGridFromGrid(GameObject grid)
    {
        //Grid değilse return et
        if (grid.GetComponent<Grid>() == null)
        {
            return;
        }
        
        //önceki grid'in scale shader'ını resetle
        selectedGrid.GetComponent<Grid>().MaterialController.ResetOutlineScale();
        selectedGrid = grid;
        selectedGrid.GetComponent<Grid>().MaterialController.SetOutlineScale();

        //search ediyorsak.
        if (IsInSearchState)
        {
            if (selectedGrid.GetComponent<Grid>().IsAvailable)
            {
                PlayerManager.Instance.SetNewGridForSelectedPlayer(selectedGrid);
            }
            else
            {
                PlayerManager.Instance.DeselectPlayer();
            }
            IsInSearchState = false;
            ResetTable();
        }
        
        if (selectedGrid.GetComponent<Grid>().GridObject != null)
        {
            var gridObject = selectedGrid.GetComponent<Grid>().GridObject;
            
            //şuan sadece player var
            if (gridObject.GetComponent<Player>()!=null)
            {
                gridObject.GetComponent<Player>().SetSelectedPlayerFromOutside();
            }
        }
    }
}

/*
 * public void SetSelectedGrid(GameObject Grid)
    {
        var gridScript = Grid.GetComponent<Grid>();
        
        if (gridScript.GridObject != null)
        {
            if (gridScript.GridObject.GetComponent<Player>() != null)
            {
                PlayerManager.Instance.SetSelectedPlayer(gridScript.GridObject);
            }
        }
        
        if (selectedGrid == null || PlayerManager.Instance.GetSelectedPlayer() == null)
        {
            //Bu demek oluyor ki biz bir adet boş grid'e tıklamışız. Ozaman bir adet UI açılır.
            //Belki bu gridde önemli birşeyler vardır. ve onu gösteririz
        }
        if (selectedGrid == Grid)
        {
            return;
        }

        if (selectedGrid != null)
        {
            selectedGrid.GetComponent<Grid>().MaterialController.ResetOutlineScale();
        }
        selectedGrid = Grid;
        selectedGrid.GetComponent<Grid>().MaterialController.SetOutlineScale();
        //Burada zaten bu fonksiyon Player Manager tarafından çağırılıyorsa ozaman
        //SetPlayerPosition() fonksiyonunda hemen geri dönecek çünkü oradaki grid ile buraki grid aynı
        PlayerManager.Instance.SetPlayerPosition(selectedGrid);
    }
 */