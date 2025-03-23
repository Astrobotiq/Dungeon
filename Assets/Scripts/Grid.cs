using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public MaterialController MaterialController;
    private List<Vector3> _nearNodes = new List<Vector3>();
    public bool IsAvailable;
    public GameObject GridObject = null;
    public int GridValue;

    void Start()
    {
        MaterialController = new MaterialController(GetComponent<Renderer>(), 0.5f);
    }

    public void setNearNodes(List<Vector3> input_arraylist){
        _nearNodes=input_arraylist;
    }
    public List<Vector3> getNearNodes(){
        return _nearNodes;
    }

    //Bu fonksiyon grid clickable'dan gelecek
    public void SetSelectedGrid()
    {
        GridManager.Instance.SetSelectedGridFromGrid(this.gameObject);
    }
    
}