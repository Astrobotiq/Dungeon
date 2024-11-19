using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private List<Vector3> nearNodes = new List<Vector3>();
    private bool isAvailable;
    

    public Grid()
    {
    }
    
    public void setNearNodes(List<Vector3> input_arraylist){
        nearNodes=input_arraylist;
    }
    public List<Vector3> getNearNodes(){
        return nearNodes;
    }
    void OnEnable()
    {
        //GridManager.Instance.AddGrid(transform.localPosition, this.gameObject);
    }
    
}