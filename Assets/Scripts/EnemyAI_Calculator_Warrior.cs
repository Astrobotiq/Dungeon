using System;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class EnemyAI_Calculator_Warrior : MonoBehaviour
{

    #region HitRewards
    
        [SerializeField]
        private int playerHitValue = 5;
    
        [SerializeField]
        private int enemyHitValue = -2;
    
        [SerializeField]
        private int statueHitValue = 5;
    
    #endregion
    
    [SerializeField]
    private int mapEdgeValue  = -99;
    
    [SerializeField]
    private List<AttackedObjectType> allAttackableObjectTypes;


    #region SİLİNECEKLER
    
        public GameObject Player;
        public GameObject Enemy;
        public bool updateUIValues = false;
    
    #endregion
    
    public enum AttackedObjectType {
        PlayerType,
        EnemyType,
        StatueType
    }

    public void Start() { // Eger yeni enum varsa buradan eklemek lazim
        allAttackableObjectTypes.Add(AttackedObjectType.PlayerType);
        allAttackableObjectTypes.Add(AttackedObjectType.EnemyType);
        //allAttackableObjectTypes.Add(AttackedObjectType.StatueType);
        
        //Silinmesi lazım
        Enemy = GameObject.Find("EnemyDummy(Clone)");
    }

    private void Update() { // Tamamen check etmek icin kullandigim bool degeri icin burada 
        
        if (updateUIValues) {
            CalculateGridMoveValues(Player);
            
            foreach (AttackedObjectType type in allAttackableObjectTypes) {
                CalculateGridAttackValues(type);
            }
            
            updateUIValues = false;
        }
    }

    public void CalculateGridAttackValues(AttackedObjectType type) {
        GridManager gridManager = GridManager.Instance;

        Vector3 position = new Vector3();
        switch (type)
        {
            case AttackedObjectType.PlayerType:
                position = Player.transform.position;
                break;
            case AttackedObjectType.EnemyType:
                position = Enemy.transform.position;
                break;
            case AttackedObjectType.StatueType:
                //Buraya statue'nun konumu gelmesi lazım
                Debug.Log("Hocam Status şeysini unttun");
                break;
        }
        
        Grid inputGridConverted = gridManager.getGridFromLocation(position);

        if (inputGridConverted.getNearNodes().Count == 0) {
            calculateNearNodes(position);
        }
        
        foreach (Vector3 grid in inputGridConverted.getNearNodes()) {
            Grid temp = gridManager.getGridFromLocation(grid);
            GameObject gridCanvas = temp.transform.GetChild(0).gameObject;
            TextMeshProUGUI textObject = gridCanvas.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            
            switch (type)
            {
                case AttackedObjectType.PlayerType:
                    textObject.text = (Int32.Parse(textObject.text) + playerHitValue).ToString();
                    break;
                case AttackedObjectType.EnemyType: 
                    textObject.text = (Int32.Parse(textObject.text) + enemyHitValue).ToString();
                    break;
                case AttackedObjectType.StatueType:
                    textObject.text = (Int32.Parse(textObject.text) + statueHitValue).ToString();
                    break;
            }
        }
    }

    public void CalculateGridMoveValues(GameObject player){
        Vector3 playerPos = player.transform.position;
        workOnNearNodes(playerPos,playerHitValue);
    }

    public void workOnNearNodes(Vector3 inputGrid, int value) {
        GridManager gridManager = GridManager.Instance;

        Grid inputGridConverted = gridManager.getGridFromLocation(inputGrid);
        
        foreach (Vector3 grid in inputGridConverted.getNearNodes()) {
            setGridUIValue(gridManager.getGridFromLocation(grid),value);
            if (value > 1) {
                workOnNearNodes(grid,value-1);
            }
        }
    }

    public void setGridUIValue(Grid gridGameobject, int value) {
        GameObject gridCanvas = gridGameobject.transform.GetChild(0).gameObject;
        TextMeshProUGUI textObject = gridCanvas.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

        if (Int32.Parse(textObject.text) > -1 && Int32.Parse(textObject.text) < value) {
            textObject.SetText(value.ToString());
            //Debug.Log("grid location: " + gridGameobject.transform.position + " and value: " + textObject.text);
        }
        
        else if (gridGameobject.GridObject && gridGameobject.GridObject.GetComponent<EnemyHealth>()) {
            textObject.SetText(enemyHitValue.ToString());
        }
        
        if(gridGameobject.transform.position.x==0 || gridGameobject.transform.position.x==7
            || gridGameobject.transform.position.z==0 || gridGameobject.transform.position.z==7) 
        {
            textObject.SetText(mapEdgeValue.ToString());
        }
    }
    
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
            /*if(location.z==7){ // x=0 y=7 ise
                nearNodes.Add(new Vector3(location.x,location.y,location.z-1)); //sol
                nearNodes.Add(new Vector3(location.x-1,location.y,location.z)); //aşşağı
            }*/
            //else{
                nearNodes.Add(new Vector3(location.x+1,location.y,location.z)); //sağ
                nearNodes.Add(new Vector3(location.x,location.y,location.z+1)); //yukarı
                nearNodes.Add(new Vector3(location.x,location.y,location.z-1)); //aşşağı
            //}
        }
        else if (location.x==7){ // x=7 ise
            /*if(location.z==0){ // hem x=7 y=0 ise
                nearNodes.Add(new Vector3(location.x,location.y,location.z+1)); //sağ
                nearNodes.Add(new Vector3(location.x+1,location.y,location.z)); //yukarı
            }*/
            //else{
                nearNodes.Add(new Vector3(location.x-1,location.y,location.z)); //sol
                nearNodes.Add(new Vector3(location.x,location.y,location.z+1)); //yukarı
                nearNodes.Add(new Vector3(location.x,location.y,location.z-1)); //aşşağı
            //}
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
