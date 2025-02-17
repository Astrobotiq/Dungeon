using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI_Organizer : MonoBehaviour
{
    #region GameObjectHolders
    
    [Header("GameobjectHolders")]
    
    [SerializeField] 
    private List<GameObject> players;

    [SerializeField] 
    private List<GameObject> statues;

    [SerializeField] 
    private List<GameObject> enemies;
    
    #endregion
    

    #region EnemyValues
    
    [Header("Enemy")]
    
    [SerializeField]
    private GameObject chosenEnemy;
    
    [SerializeField] 
    private int enemyChooseRange;
    
    #endregion

    #region General
    
    [SerializeField]
    private List<EnemyAI_Calculator_Warrior.AttackedObjectType> allAttackableObjectTypes;
    
    private EnemyAI_Calculator_Warrior enemyAICalculator;
    
    #endregion
    
    //SİLİNECEK
    public bool showSelectedOptions;
    
    public void Start() // SİLİNECEK
    {
        chosenEnemy = GameObject.Find("EnemyDummy(Clone)");
        enemies.Add(chosenEnemy); // !!!!! Kendini eklememesi lazım ama check etmek için şimdilik ekledim
        
        
        allAttackableObjectTypes.Add(EnemyAI_Calculator_Warrior.AttackedObjectType.PlayerType);
        allAttackableObjectTypes.Add(EnemyAI_Calculator_Warrior.AttackedObjectType.EnemyType);
        //allAttackableObjectTypes.Add(EnemyAI_Calculator_Warrior.AttackedObjectType.StatueType);
    }

    public void Update() { // Bool degeri uzerinden yaptigim check icin burada
        if (enemyAICalculator == null) {
            //Burada get component ile alınan componeneti abstract bir classın child'ı yapmak istiyorum ki içine verilen claasın kurallarına göre hesap yapsın yani archer da yapılınca değişecek
            enemyAICalculator = GetComponent<EnemyAI_Calculator_Warrior>(); 
        }
        
        if (showSelectedOptions) {
             Vector3 temp = ReturnBestOption(chosenEnemy); // This is for returning the single best location
             Debug.Log("seçtiğim en iyi loc " + temp);
        }
    }

    public Vector3 ReturnBestOption(GameObject enemy)
    {
        Dictionary<Vector3, int> TotalOptions = new Dictionary<Vector3, int>();
        
        chosenEnemy = enemy;
        
        foreach (EnemyAI_Calculator_Warrior.AttackedObjectType type in allAttackableObjectTypes)
        {
            CalculateAllGridValues(type);
        }
        
        TotalOptions = DecideBestActions_3();
        
        foreach (var VARIABLE in TotalOptions) {
            Debug.Log("bulduğum en iyi loc biri : " + VARIABLE.Key + " sayısal değeri " + VARIABLE.Value);
        }
            
        Vector3 bestLoc = new Vector3(99, 99, 99);
        int bestValue = -99;
            
        foreach (KeyValuePair<Vector3, int> tile in TotalOptions) {
            //Debug.Log("en iyi sayı " + bestValue + " şu anki sayı " + tile.Value);
            if (tile.Value > bestValue) {
                bestLoc = tile.Key;
                bestValue = tile.Value;
            }
        }
        //Debug.Log("boş");
        //Debug.Log("bulduğum en iyi loc " + bestLoc + " değeri " + bestValue);
        showSelectedOptions = false; // SİLİNECEK
        
        return bestLoc;
    }
    
    public void CalculateAllGridValues(EnemyAI_Calculator_Warrior.AttackedObjectType type){
        switch (type) 
        {
            case EnemyAI_Calculator_Warrior.AttackedObjectType.PlayerType:
                if (players.Count != 0) {
                    foreach (GameObject player in players) {
                        enemyAICalculator.CalculateGridMoveValues(player, type);
                        enemyAICalculator.CalculateGridAttackValues(player, type);
                    }
                }
                else {
                    Debug.Log("EnemyAIOrganizer's players list is empty"); 
                }
                break;
            
            case EnemyAI_Calculator_Warrior.AttackedObjectType.EnemyType:
                if (enemies.Count != 0) {
                    foreach (GameObject enemy in enemies) { // !!!!!!!! I think there is no need to calc move value addition by enemies
                        enemyAICalculator.CalculateGridMoveValues(enemy, type);
                        enemyAICalculator.CalculateGridAttackValues(enemy, type);
                    }
                }
                else {
                    Debug.Log("EnemyAIOrganizer's enemies list is empty");
                }
                break;
            
            case EnemyAI_Calculator_Warrior.AttackedObjectType.StatueType:
                if (statues.Count != 0) {
                    foreach (GameObject statue in statues) {
                        enemyAICalculator.CalculateGridMoveValues(statue, type);
                        enemyAICalculator.CalculateGridAttackValues(statue, type);
                    }
                }
                else {
                    Debug.Log("EnemyAIOrganizer's statues list is empty"); 
                }
                break;
            default:
                Debug.Log("Calling from EnemyAI_Organizer. I think you refer a type which is not put in here");
                break;
        }
    }
    
    public Dictionary<Vector3, int> DecideBestActions_3(){
        GridManager gridManager = GridManager.Instance;
        Algorithm algorithm = new Algorithm();
        
        HashSet<Vector3> lookableTiles = algorithm.startAlgorithm(gridManager.getGridFromLocation(chosenEnemy.transform.position), enemyChooseRange);
        Dictionary<Vector3, int> dictionaryTiles = new Dictionary<Vector3, int>();
        
        
        //Debug.Log("lookabletile sayısı " + lookableTiles.Count);
        foreach (Vector3 grid in lookableTiles) {
            Grid temp = gridManager.getGridFromLocation(grid);
            GameObject grid_canvas = temp.transform.GetChild(0).gameObject;
            TextMeshProUGUI text_object = grid_canvas.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

            int gridValue = Int32.Parse(text_object.text);
            dictionaryTiles.Add(grid, gridValue);
        }
        //Debug.Log("dictionarytile sayısı " + dictionaryTiles.Count);
        
        Dictionary<Vector3, int> bestThreeOption = new Dictionary<Vector3, int>();

        for (int i = 0; i < 3; i++) {
            Vector3 bestLoc = new Vector3(99, 99, 99);
            int bestValue = -99;
            
            foreach (KeyValuePair<Vector3, int> tile in dictionaryTiles) {
                //Debug.Log("en iyi sayı " + bestValue + " şu anki sayı " + tile.Value);
                if (tile.Value > bestValue) {
                    bestLoc = tile.Key;
                    bestValue = tile.Value;
                }
            }
            //Debug.Log("boş");
            //Debug.Log("bulduğum en iyi loc " + bestLoc + " değeri " + bestValue);
            bestThreeOption.Add(bestLoc, bestValue);
            dictionaryTiles.Remove(bestLoc);
        }
        
        /*Debug.Log("option sayısı " + bestThreeOption.Count);
        foreach (var VARIABLE in bestThreeOption) {
           Debug.Log("!!! optıon loc degerı " + VARIABLE.Key + " option sayısal değeri " + VARIABLE.Value);
        }*/
        
        return bestThreeOption;
    }
}

/*
public class EnemyAI_Organizer : MonoBehaviour
{
    [SerializeField] 
    private List<GameObject> players;

    #region EnemyValues
    
    [Header("Enemy")]
    
    [SerializeField]
    private GameObject chosenEnemy;
    
    [SerializeField] 
    private int enemyChooseRange;
    
    #endregion
    
    //SİLİNECEK
    public bool showSelectedOptions;
    private EnemyAI_Calculator_Warrior enemyAICalculator;

    public Dictionary<Vector3, (int, GameObject)> TotalOptions;
    
    public void Start() // SİLİNECEK
    {
        players.Add(GameObject.Find("Player(Clone)"));
        chosenEnemy = GameObject.Find("EnemyDummy(Clone)");
        
        TotalOptions = new Dictionary<Vector3, (int, GameObject)>();
    }

    public void Update() { // Bool degeri uzerinden yaptigim check icin burada
        if (enemyAICalculator == null) {
            //Burada get component ile alınan componeneti abstract bir classın child'ı yapmak istiyorum ki içine verilen claasın kurallarına göre hesap yapsın
            enemyAICalculator = GetComponent<EnemyAI_Calculator_Warrior>(); 
        }
        
        if (showSelectedOptions) {
            foreach (GameObject player in players) {
                enemyAICalculator.CalculateGridMoveValues(player); // Bunun içine player atilacak sıra sıra. Her player için de bu 3 işlem yapılacak
                
                enemyAICalculator.CalculateGridAttackValues(EnemyAI_Calculator_Warrior.AttackedObjectType.PlayerType);
                //enemyAICalculator.CalculateGridAttackValues(EnemyAI_Calculator_Warrior.AttackedObjectType.EnemyType);
                
                Dictionary<Vector3, int> temp = DecideBestActions_3();
                
                foreach (KeyValuePair<Vector3,int> returned_options in temp) {
                    if (TotalOptions.ContainsKey(returned_options.Key) && TotalOptions[returned_options.Key].Item1 < returned_options.Value) {
                        TotalOptions[returned_options.Key] = (returned_options.Value, TotalOptions[returned_options.Key].Item2);
                    }
                    else {
                        //Debug.Log("uğradım uğradım uğradım");
                        TotalOptions.Add(returned_options.Key, (returned_options.Value, player));
                    }
                }
            }

            foreach (var VARIABLE in TotalOptions) {
                Debug.Log("gideceğim loc: " + VARIABLE.Key + " sayısal değeri " + VARIABLE.Value.Item1 + " vuracağım gameobject " + VARIABLE.Value.Item2);
            }
            // Buradan devam et burada random şekilde seçme işini yapman lazım + test etmeyi unutma çalışıp çalışmadığını

            showSelectedOptions = false;
        }
    }
    
    public Dictionary<Vector3, int> DecideBestActions_3(){
        GridManager gridManager = GridManager.Instance;
        Algorithm algorithm = new Algorithm();
        
        HashSet<Vector3> lookableTiles = algorithm.startAlgorithm(gridManager.getGridFromLocation(chosenEnemy.transform.position), enemyChooseRange);
        Dictionary<Vector3, int> dictionaryTiles = new Dictionary<Vector3, int>();
        
        
        //Debug.Log("lookabletile sayısı " + lookableTiles.Count);
        foreach (Vector3 grid in lookableTiles) {
            Grid temp = gridManager.getGridFromLocation(grid);
            GameObject grid_canvas = temp.transform.GetChild(0).gameObject;
            TextMeshProUGUI text_object = grid_canvas.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

            int gridValue = Int32.Parse(text_object.text);
            dictionaryTiles.Add(grid, gridValue);
        }
        //Debug.Log("dictionarytile sayısı " + dictionaryTiles.Count);
        
        Dictionary<Vector3, int> bestThreeOption = new Dictionary<Vector3, int>();

        for (int i = 0; i < 3; i++) {
            Vector3 bestLoc = new Vector3(99, 99, 99);
            int bestValue = -99;
            
            foreach (KeyValuePair<Vector3, int> tile in dictionaryTiles) {
                //Debug.Log("en iyi sayı " + bestValue + " şu anki sayı " + tile.Value);
                if (tile.Value > bestValue) {
                    bestLoc = tile.Key;
                    bestValue = tile.Value;
                }
            }
            //Debug.Log("boş");
            //Debug.Log("bulduğum en iyi loc " + bestLoc + " değeri " + bestValue);
            bestThreeOption.Add(bestLoc, bestValue);
            dictionaryTiles.Remove(bestLoc);
        }
        
        /*Debug.Log("option sayısı " + bestThreeOption.Count);
        foreach (var VARIABLE in bestThreeOption) {
           Debug.Log("!!! optıon loc degerı " + VARIABLE.Key + " option sayısal değeri " + VARIABLE.Value);
        }#1#
        
        return bestThreeOption;
    }
    
    //Bu alttaki DecideBEstAction, DecideBestAction_2 ve Changer SİLİNEBİLİR aslında ama şimdilik tutuyorum
    public Dictionary<Vector3,int> DecideBestActions() {
        GridManager gridManager = GridManager.Instance;
        Algorithm algorithm = new Algorithm();
        //List<List<GameObject>> tempGridList = gridManager.GridList; // Bunun yerine enemy'nin yuruyebildigi gridleri tutan bir list lazim

        Dictionary<Vector3, int> options = new Dictionary<Vector3, int>();
        options.Add(new Vector3(99, 99, 99), -99);
        options.Add(new Vector3(98, 98, 98), -99);
        options.Add(new Vector3(97, 97, 97), -99);
        
        HashSet<Vector3> lookableTiles = algorithm.startAlgorithm(gridManager.getGridFromLocation(chosenEnemy.transform.position), enemyChooseRange);

        // foreach (var VARIABLE in lookableTiles)
        // {
        //     Debug.Log("Enemy gördüğü tilelar " + VARIABLE);
        // }
        
        Dictionary<Vector3, int> temp_dic = new Dictionary<Vector3, int>();
        foreach (var VARIABLE in options) {
            temp_dic.Add(VARIABLE.Key,VARIABLE.Value);
        }

        //int count_inner = 0;
        //int count_outer = 0;
        /*foreach (KeyValuePair<Vector3, int> temp_option in temp_dic)
        {
            bool buldum = false;
            bool silmekLazım = false;
            
            foreach (Vector3 tile in lookableTiles) {
                Grid temp = gridManager.getGridFromLocation(tile);
                GameObject grid_canvas = temp.transform.GetChild(0).gameObject;
                TextMeshProUGUI text_object = grid_canvas.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

                int gridValue = Int32.Parse(text_object.text);

                int bestChoiceValue = 0;
                Vector3 bestChoiceLocation = new Vector3();

                if (!options.ContainsKey(tile)) {
                    if (temp_option.Value < gridValue)
                    {
                        bestChoiceLocation = tile;
                        bestChoiceValue = gridValue;
                        
                        // options.Remove(temp_option.Key);
                        // options.Add(tile, gridValue);
                        // lookableTiles.Remove(tile);
                        // break;
                        buldum = true;
                        silmekLazım = true;
                    }
                }
                
                else {
                    if (options[tile] < gridValue) {
                        options.Remove(temp_option.Key);
                        options.Add(tile, gridValue);
                        buldum = true;
                    }
                }

                if (buldum) {
                    options.Remove(temp_option.Key);
                    options.Add(tile, gridValue);
                    if (silmekLazım) {
                        lookableTiles.Remove(tile);
                        break;
                    }
                    
                }
                Debug.Log("naber inner " + count_inner);
                count_inner +=1;
                
                foreach (var VARIABLE in options)
                {
                    Debug.Log("????? optıon loc degerı " + VARIABLE.Key + " option sayısal değeri " + VARIABLE.Value);
                }
            }
            
            Debug.Log("naber outer " + count_inner);
            count_outer +=1;
            
            foreach (var VARIABLE in options)
            {
                Debug.Log("????? optıon loc degerı " + VARIABLE.Key + " option sayısal değeri " + VARIABLE.Value);
            }
        }
        
        Debug.Log("sondan önce");
        foreach (var VARIABLE in options)
        {
            Debug.Log("????? optıon loc degerı " + VARIABLE.Key + " option sayısal değeri " + VARIABLE.Value);
        }#1#
        
        /*foreach (Vector3 grid in lookableTiles) {
            Grid temp = gridManager.getGridFromLocation(grid);
            GameObject grid_canvas = temp.transform.GetChild(0).gameObject;
            TextMeshProUGUI text_object = grid_canvas.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

            int gridValue = Int32.Parse(text_object.text);
            
            //Debug.Log("görülen grid pos " + grid + " sayısal değeri " + gridValue);

            Dictionary<Vector3, int> temp_dic = new Dictionary<Vector3, int>();
            foreach (var VARIABLE in options) {
                temp_dic.Add(VARIABLE.Key,VARIABLE.Value);
            }
            
            foreach (KeyValuePair<Vector3,int> temp_option in temp_dic) {
                Debug.Log("optıon loc degerı " + temp_option.Key + " option sayısal değeri " + temp_option.Value);
                Debug.Log("grid loc degerı " + grid+ " grid sayısal değeri " + gridValue);

                if (options.ContainsKey(grid))
                {
                    if (gridValue > options[temp_option.Key])
                    {
                        options.Remove(temp_option.Key);
                        options.Add(grid, gridValue);
                    }
                }
                else if (gridValue > temp_option.Value){
                    options.Remove(temp_option.Key);
                    options.Add(grid, gridValue);
                }
            }

            foreach (var VARIABLE in options)
            {
                Debug.Log("????? optıon loc degerı " + VARIABLE.Key + " option sayısal değeri " + VARIABLE.Value);
            }
            
        }#1#
        
        return options;
    }

    public Dictionary<Vector3, int> DecideBestActions_2() {
        
        GridManager gridManager = GridManager.Instance;
        Algorithm algorithm = new Algorithm();

        Dictionary<Vector3, int> options = new Dictionary<Vector3, int>();

        HashSet<Vector3> lookableGrids = algorithm.startAlgorithm(gridManager.getGridFromLocation(chosenEnemy.transform.position), enemyChooseRange);
        /*
        foreach (Vector3 grid in lookableGrids) {
            Grid temp = gridManager.getGridFromLocation(grid);
            GameObject grid_canvas = temp.transform.GetChild(0).gameObject;
            TextMeshProUGUI text_object = grid_canvas.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

            int gridValue = Int32.Parse(text_object.text);

            List<Changer> changes = new List<Changer>();
            if (options.Count == 0) {
                options.Add(grid,gridValue);
            }
            else
            {
                foreach (var VARIABLE in options) {
                    if (gridValue > VARIABLE.Value) {
                        Changer naber = new Changer();
                        naber.changedLoc = VARIABLE.Key;
                        naber.newLoc = grid;
                        naber.newValue = gridValue;
                        changes.Add(naber);
                    }
                }
            }

            if (changes.Count != 0) {
                Changer best = changes[0];
                foreach (var naber in changes) {
                    if (naber.newValue > best.newValue) {
                        best = naber;
                    }
                }

                options.Remove(best.changedLoc);
                options.Add(best.newLoc, best.newValue);
            }

            if (changes.Count != 0) {
                Changer best = changes[0];
                foreach (var naber in changes) {
                    if (naber.newValue > best.newValue) {
                        best = naber;
                    }
                }

                options.Remove(best.changedLoc);
                options.Add(best.newLoc, best.newValue);
            }

            if (changes.Count != 0) {
                Changer best = changes[0];
                foreach (var naber in changes) {
                    if (naber.newValue > best.newValue) {
                        best = naber;
                    }
                }

                options.Remove(best.changedLoc);
                options.Add(best.newLoc, best.newValue);
            }

            foreach (var VARIABLE in options)
            {
                Debug.Log("????? optıon loc degerı " + VARIABLE.Key + " option sayısal değeri " + VARIABLE.Value);
            }


        }
        #1#
        return options;
    }

    class Changer
    {
        public Vector3 changedLoc;
        public Vector3 newLoc;
        public int newValue;
    }
    
}
*/