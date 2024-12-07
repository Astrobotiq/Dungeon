using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public MaterialController MaterialController;
    
    private List<Vector3> nearNodes = new List<Vector3>();

    [SerializeField] private bool isAvailable;

    void Start(){
        MaterialController = new MaterialController(GetComponent<Renderer>(), 0.5f);
        isAvailable = true;
    }

    public void checkIsThereObjectUpside(){ //Üzerinde bir obje varsa isAvailable değerini false'a çekmesi için.
        //Normalde getIsAvailable methodunun başında bir kere çağırmayı düşündüm ama test ederken player objesine çarptığı için test etmemi engelliyordu ondan şimdilik yoruma çevirdim
        //(Code review da konuşulacak)
        RaycastHit temp;
        Physics.Raycast(gameObject.transform.position, transform.up, out temp, 5);
        if (temp.transform.gameObject!=null){
            setIsAvailable(false);
        }
    }

    public void setNearNodes(List<Vector3> input_arraylist){
        nearNodes=input_arraylist;
    }
    public List<Vector3> getNearNodes(){
        return nearNodes;
    }
    
    public void setIsAvailable(bool input_bool){
        isAvailable = input_bool;
    }

    public bool getIsAvailable(){
        //checkIsThereObjectUpside(); //(Code review da konuşulacak)
        return isAvailable;
    }
    
    
    
    
}