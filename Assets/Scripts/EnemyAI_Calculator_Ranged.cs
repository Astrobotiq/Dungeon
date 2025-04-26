using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TMPro;
using UnityEngine;

public class EnemyAI_Calculator_Ranged : AbstractEnemyAI_Calculator
{
    #region Rewards
    
        [SerializeField]
        private int playerHitValue = 5;
    
        [SerializeField]
        private int enemyHitValue = -2;
    
        [SerializeField]
        private int statueHitValue = 5;
        
        [SerializeField]
        private int mapEdgeValue  = -99;
    
    #endregion
    
    //SİLİNECEK
    // [SerializeField] 
    // private GameObject enemy;
    //
    // [SerializeField] 
    // private GameObject player;
    //
    // [SerializeField] 
    // private bool showArcherValues;
    //
    // [SerializeField] 
    // private int enemyWalkDistance = 2;
    //
    private Algorithm alg;

    [SerializeField] 
    private int enemyMovePunish = -1;
    
    public List<Vector3> lookedGrids;
    
    
    public void Start() //SİLİNECEK
    {
        // enemy = GameObject.Find("EnemyDummy(Clone)");
        // player = transform.gameObject;
        // alg = new Algorithm();
        //
        // alg.startAlgorithm(GridManager.Instance.getGridFromLocation(enemy.transform.position), enemyWalkDistance+1);
        lookedGrids = new List<Vector3>();
    }

    // public void Update() {
    //     if (showArcherValues) {
    //         CalculateGridAttackValues(player, AttackedObjectType.PlayerType);
    //         CalculateGridMoveValues(enemy, AttackedObjectType.EnemyType, enemyWalkDistance);
    //         
    //         showArcherValues = false;
    //     }
    // }

    public override void CalculateGridAttackValues(GameObject gameObject, AttackedObjectType type) {
        if (type != AttackedObjectType.PlayerType) {
            return;
        }
        
        GridManager gridManager = GridManager.Instance;

        Vector3 position = gameObject.transform.position;
        Grid inputGridConverted = gridManager.getGridFromLocation(position);
        Vector3 location = inputGridConverted.gameObject.transform.position;
        
        workOnHorizontalNVertical(location,7);
    }

    public void workOnHorizontalNVertical(Vector3 location, int input_distance) {
        GridManager gridManagerInstance = GridManager.Instance;
        
        List<Vector3> gridListVec3 = new List<Vector3>();
        
        int minXCanGo=(int)location.x - input_distance; 
        int maxXCanGo=(int)location.x + input_distance;
        int minZCanGo=(int)location.z - input_distance;
        int maxZCanGo=(int)location.z + input_distance;
        

        if (maxXCanGo > 7) maxXCanGo = 7;
        if (minXCanGo < 0) minXCanGo = 0;
        if (maxZCanGo > 7) maxZCanGo = 7;
        if (minZCanGo < 0) minZCanGo = 0;
        
        // Below if part is for gathering available grids on 4 directions
        if (!(location.x == 7)) {
            for (int i = (int)location.x; i <= maxXCanGo; i++) {
                Grid temp_grid = gridManagerInstance.getGridFromLocation(new Vector3(i, location.y, location.z));
                if (temp_grid.GridObject == null) {
                    gridListVec3.Add(new Vector3(i, location.y, location.z));
                }
            }
        }
        if (!(location.x == 0)) {
            for (int i = (int)location.x; i >= minXCanGo; i--) {
                Grid temp_grid = gridManagerInstance.getGridFromLocation(new Vector3(i, location.y, location.z));
                if (temp_grid.GridObject == null){ 
                    gridListVec3.Add(new Vector3(i, location.y, location.z));
                }
            }
        }
        if (!(location.z == 7)) {
            for (int i = (int)location.z; i <= maxZCanGo; i++) {
                Grid temp_grid = gridManagerInstance.getGridFromLocation(new Vector3(location.x, location.y, i));
                if (temp_grid.GridObject == null){ 
                    gridListVec3.Add(new Vector3(location.x, location.y, i));
                }
            }
        }
        if (!(location.z == 0)) {
            for (int i = (int)location.z; i >= minZCanGo; i--) {
                Grid temp_grid = gridManagerInstance.getGridFromLocation(new Vector3(location.x, location.y, i));
                if (temp_grid.GridObject == null){ 
                    gridListVec3.Add(new Vector3(location.x, location.y, i));
                }
            }
        }
        
        //Debug.Log(gridListVec3.Count);

        foreach (Vector3 loc in gridListVec3) {
            setGridUIValueAttack(gridManagerInstance.getGridFromLocation(loc), playerHitValue);
        }
    }
    
    public void setGridUIValueAttack(Grid gridReference, int value) {
        //GameObject gridCanvas = gridReference.transform.GetChild(0).gameObject;
        //TextMeshProUGUI textObject = gridCanvas.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

        //textObject.SetText( (Int32.Parse(textObject.text) + value) .ToString());
        gridReference.GridValue += value;
    }
    
    public override void CalculateGridMoveValues(GameObject gameObject, AttackedObjectType type, int enemyWalkDistance) {

        if (type != AttackedObjectType.EnemyType) {
            return;
        }

        if (lookedGrids.Count > 0) { lookedGrids.Clear(); } 
        
        Vector3 pos = gameObject.transform.position;
        workOnNearNodes(pos, pos, enemyWalkDistance);
    }

    public void workOnNearNodes(Vector3 inputGrid, Vector3 enemyRef, int enemyWalkDistance) {
        GridManager gridManager = GridManager.Instance;

        Grid inputGridConverted = gridManager.getGridFromLocation(inputGrid);

        if (inputGridConverted.getNearNodes().Count < 1 & !lookedGrids.Contains(inputGrid)) {
            calculateNearNodes(inputGrid);
        }
        
        lookedGrids.Add(new Vector3(inputGrid.x, 0, inputGrid.z));
        //Debug.Log("ekledim " + new Vector3(inputGrid.x, 0, inputGrid.z));
        
        foreach (Vector3 grid in inputGridConverted.getNearNodes()) {
            //Debug.Log("atam olan grid " + inputGrid + " ben de " + grid);
            int temp = distanceFromEnemyGrid(grid, enemyRef);
            int temp_mult = temp * enemyMovePunish;
            
            if (Mathf.Abs(temp) <= enemyWalkDistance && !lookedGrids.Contains(grid)) {
                //lookedGrids.Add(grid);
                setGridUIValueMove(gridManager.getGridFromLocation(grid), temp_mult);
                //Debug.Log("sayiyi da yazdim");
                //workOnNearNodes(grid, enemyRef, enemyWalkDistance);
            }
        }
    }

    public int distanceFromEnemyGrid(Vector3 grid, Vector3 enemyRef) {
        //Debug.Log("malum sayi " + (int) (Mathf.Abs(enemyRef.x - grid.x) + Mathf.Abs(enemyRef.z - grid.z)));
        return (int) (Mathf.Abs(enemyRef.x - grid.x) + Mathf.Abs(enemyRef.z - grid.z));
    }

    public void setGridUIValueMove(Grid gridReference, int value) {
        // GameObject gridCanvas = gridReference.transform.GetChild(0).gameObject;
        // TextMeshProUGUI textObject = gridCanvas.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        
        if(gridReference.transform.position.x==0 || gridReference.transform.position.x==7
            || gridReference.transform.position.z==0 || gridReference.transform.position.z==7) 
        {
            //textObject.SetText(mapEdgeValue.ToString());
            gridReference.GridValue = mapEdgeValue;
        }
        else if(lookedGrids.Contains(gridReference.transform.position)) {
            
        }
        else {
            //Debug.Log("suanki degeri " + Int32.Parse(textObject.text) + " ben ekliyecegim " + value);
            //textObject.SetText( (Int32.Parse(textObject.text) + value) .ToString());
            //Debug.Log("suanki degeri " + gridReference + " ben ekliyecegim " + value);
            gridReference.GridValue += value;
        }
    }

    /*public override void CalculateGridMoveValues(GameObject gameObject, EnemyAI_Calculator_Warrior.AttackedObjectType type) {

        if (type != AttackedObjectType.EnemyType)
        {
            return;
        }

        List<Vector3> allLookedGrids = new List<Vector3>();
        
        Debug.Log("bana gelen enum degeri " + type + "game object de " + gameObject);
        int value = 0;
        
        if (type == AttackedObjectType.EnemyType)
        {
           value =  enemyMovePunish;
           Vector3 pos = gameObject.transform.position;
           workOnNearNodes(pos, value, allLookedGrids);
           
        }
        
        GridManager.Instance.getGridFromLocation(gameObject.transform.position).
            transform.GetChild(0)
            .gameObject.transform.GetChild(0)
            .gameObject.GetComponent<TextMeshProUGUI>().SetText("0");
        
       /*int value = 0;

       switch (type)
       {
           case EnemyAI_Calculator_Warrior.AttackedObjectType.PlayerType:
               value = playerHitValue;
               break;
           case EnemyAI_Calculator_Warrior.AttackedObjectType.EnemyType:
               value = enemyHitValue;
               break;
           case EnemyAI_Calculator_Warrior.AttackedObjectType.StatueType:
               value = statueHitValue;
               break;
           default:
               Debug.Log("Calling from EnemyAICalculator_Warrior. I think you refer a type which is not put in here");
               break;
       }

       Vector3 pos = gameObject.transform.position;
       workOnNearNodes(pos,value);#1#
    }
    
    public void workOnNearNodes(Vector3 inputGrid, int value, List<Vector3> lookedGrids) {
        GridManager gridManager = GridManager.Instance;

        Grid inputGridConverted = gridManager.getGridFromLocation(inputGrid);
        
        Debug.Log("sunun near node bulacagim " + inputGrid  + " input grid near node sayisi " + inputGridConverted.getNearNodes().Count);
        
        foreach (Vector3 grid in inputGridConverted.getNearNodes()) {
            Debug.Log("girdigim vec 3 " + grid);
            if (!lookedGrids.Contains(grid)) {
                setGridUIValue(gridManager.getGridFromLocation(grid),value);
                lookedGrids.Add(grid);
                if (Mathf.Abs(value) < enemyWalkDistance + 1) {
                    workOnNearNodes(grid, value - 1 , lookedGrids);
                }
            }
            
        }
    }

    public void setGridUIValue(Grid gridReference, int value) {
        GameObject gridCanvas = gridReference.transform.GetChild(0).gameObject;
        TextMeshProUGUI textObject = gridCanvas.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        
        if (gridReference.GridObject && gridReference.GridObject.GetComponent<EnemyHealth>()) {
            textObject.SetText(enemyHitValue.ToString());
        }
        else if(gridReference.transform.position.x==0 || gridReference.transform.position.x==7 
             || gridReference.transform.position.z==0 || gridReference.transform.position.z==7) 
        {
            textObject.SetText(mapEdgeValue.ToString());
        }
        else
        {
            textObject.SetText( (Int32.Parse(textObject.text) + value) .ToString());
        }
    }*/
    
    private void calculateNearNodes(Vector3 coordinates) { // Bizim eski komşu gridleri bulma methodu
        Vector3 location = coordinates;
        List<Vector3> nearNodes = new List<Vector3>();
        

        if(location.z==0){ // y=0 ise
            if(location.x==0){ //hem x hem y = 0 ise
                nearNodes.Add(new Vector3(location.x+1,location.y,location.z)); //sağ
                nearNodes.Add(new Vector3(location.x,location.y,location.z+1)); //yukarı
            }
            else if(location.x==7){ //hem x hem de y 7 ise
                nearNodes.Add(new Vector3(location.x-1,location.y,location.z)); //sol
                nearNodes.Add(new Vector3(location.x,location.y,location.z+1)); //yukarı
            }
            else{
                nearNodes.Add(new Vector3(location.x+1,location.y,location.z)); //sağ
                nearNodes.Add(new Vector3(location.x-1,location.y,location.z)); //sol
                nearNodes.Add(new Vector3(location.x,location.y,location.z+1)); //yukarı
            }
        }
        else if(location.z==7){ // y=7 ise
            if(location.x==7){ //hem x hem de y 7 ise
                nearNodes.Add(new Vector3(location.x-1,location.y,location.z)); //sol
                nearNodes.Add(new Vector3(location.x,location.y,location.z-1)); //aşşağı
            }
            else if(location.x==0){ //hem x=0 y 7 ise
                nearNodes.Add(new Vector3(location.x+1,location.y,location.z)); //sağ
                nearNodes.Add(new Vector3(location.x,location.y,location.z-1)); //aşşağı
            }
            else{
                nearNodes.Add(new Vector3(location.x+1,location.y,location.z)); //sağ
                nearNodes.Add(new Vector3(location.x-1,location.y,location.z)); //sol
                nearNodes.Add(new Vector3(location.x,location.y,location.z-1)); //aşşağı
            }
        }
        else if(location.x==0){ // x=0 ise
            if(location.z==7){ // x=0 y=7 ise
                nearNodes.Add(new Vector3(location.x,location.y,location.z-1)); //sol
                nearNodes.Add(new Vector3(location.x-1,location.y,location.z)); //aşşağı
            }
            else{
                nearNodes.Add(new Vector3(location.x+1,location.y,location.z)); //sağ
                nearNodes.Add(new Vector3(location.x,location.y,location.z+1)); //yukarı
                nearNodes.Add(new Vector3(location.x,location.y,location.z-1)); //aşşağı
            }
        }
        else if (location.x==7){ // x=7 ise
            if(location.z==0){ // hem x=7 y=0 ise
                nearNodes.Add(new Vector3(location.x,location.y,location.z+1)); //sağ
                nearNodes.Add(new Vector3(location.x+1,location.y,location.z)); //yukarı
            }
            else{
                nearNodes.Add(new Vector3(location.x-1,location.y,location.z)); //sol
                nearNodes.Add(new Vector3(location.x,location.y,location.z+1)); //yukarı
                nearNodes.Add(new Vector3(location.x,location.y,location.z-1)); //aşşağı
            }
        }
        else{
            nearNodes.Add(new Vector3(location.x+1,location.y,location.z)); //sağ
            nearNodes.Add(new Vector3(location.x-1,location.y,location.z)); //sol
            nearNodes.Add(new Vector3(location.x,location.y,location.z+1)); //yukarı
            nearNodes.Add(new Vector3(location.x,location.y,location.z-1)); //aşşağı
        }

        //Debug.Log("3-Location : " + location);
        GridManager.Instance.getGridFromLocation(location).setNearNodes(nearNodes);
        
    }
    
}
