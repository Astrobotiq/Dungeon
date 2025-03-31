using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class InGameUITextMesh : MonoBehaviour {
    
    [SerializeField] 
    private List<GameObject> players;
    
    [SerializeField]
    private List<GameObject> enemies;

    private PlayerManager playerManager;
    //private EnemyManager enemyManager;

    private GameObject publicHealth;

    [SerializeField] 
    private SoundManager soundManager;
    
    //SILINECEK
    public bool test;
    private TurnManager turnManager;

    public void Awake() {
        
        // Bu sekilde almasi lazim bu branch uzerinde playerManager, enemyManager ve levelManager icinmdeki degisiklikler yoktu
        // playerManager = PlayerManager.Instance;
        // enemyManager = EnemyManager.Instance;
        // players = playerManager.playerListForEnemyAI;
        // enemies = enemyManager.enemyListForEnemyAI;
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        publicHealth = GameObject.Find("PublicHealth");

        turnManager = new TurnManager();
    }

    public void Update() {
        
        if (test) {
            if (players.Count == 0) {
                players.Add(GameObject.Find("Player(Clone)"));
            }
            
            if (enemies.Count == 0) {
                enemies.Add(GameObject.Find("EnemyDummy(Clone)"));
            }
            
            testDecreaseValues(10);
            test = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Debug.Log("naber");
            UpdateESC_Window();
        }
    }

    public void testDecreaseValues(float value) { //SILINECEK
        updatePublicBar(- value);
        UpdatePlayerBars();
        UpdateTurnDisplay();
    }

    public void updatePublicBar(float value) {
        Slider publicBarSlider = publicHealth.transform.GetChild(0).GetComponent<Slider>();
        publicBarSlider.value += value;
        
        
        
        if (publicBarSlider.value < 0) {
            publicBarSlider.value = 0;
        }
        else if (publicBarSlider.value > 100) {
            publicBarSlider.value = 100;
        }
    }

    public void UpdatePlayerBars() {
        //players = playerManager.playerListForEnemyAI;
        
        for (int i = 0; i < players.Count; i++) {
            GameObject temp = GameObject.Find("PlayerHealth_" + (i+1) );
            temp.transform.GetChild(0).gameObject.SetActive(true); // PlayerHealth slider active eder
            temp.transform.GetChild(1).gameObject.SetActive(true); // PlayerHealth iconFrame active eder
            
            UpdatePlayerImage(temp,players[i]);
            UpdatePlayerHP(temp, players[i]);
        }
    }
    
    public void UpdatePlayerImage(GameObject PlayerBar, GameObject player) {
        Image tempImage = PlayerBar.transform.GetChild(1).GetChild(0).GetComponent<Image>();
        Player playerComponent = player.GetComponent<Player>();
        
        if (playerComponent.PlayerSprite) {
            tempImage.sprite = player.GetComponent<Player>().PlayerSprite;
        }
        else {
            Debug.Log("PlayerImage bulamadim su playerBar icin" + PlayerBar.name);
        }
        
    }

    public void UpdatePlayerHP(GameObject PlayerBar, GameObject player) {
        Slider tempSlider = PlayerBar.transform.GetChild(0).GetComponent<Slider>();
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        //Debug.Log("Slider value " + tempSlider.value + " player health " + playerHealth.getHealthPercentage() );
        
        playerHealth.TakeDamage(1); //SILINECEK
        //Debug.Log("Slider value " + tempSlider.value + " player health " + playerHealth.getHealthPercentage() );
        tempSlider.value = playerHealth.getHealthPercentage();
        
        if (tempSlider.value < 0) {
            tempSlider.value = 0;
        }
        else if (tempSlider.value > 100) {
            tempSlider.value = 100;
        }
    }
    
    // BU TAMAMLANACAK
    public void UpdateEnemyArrangement() {
        // enemies = enemyManager.enemyListForEnemyAI;
        
        for (int i = 0; i < enemies.Count; i++) {
            GameObject temp = GameObject.Find("Enemy_" + (i+1) );
            temp.transform.GetChild(0).gameObject.SetActive(true); // EnemyProfıle active eder
            temp.transform.GetChild(1).gameObject.SetActive(true); // Enemy Level (bızım ıcın oynama sırası) active eder
            
            // EnemyManager icinde buraya enemleri oynama sairasina gore dondurecek bir method lazim
        }
    }

    public void UndoButton() {
        // Burada Eren'in eventleri cagirmasi lazim Mutlaka Eren'e sor eksik bir sey var mi diye
        if (CommandManager.Instance.HasCommand())
        {
            CommandManager.Instance.Undo();
        }
        soundManager.PlaySound(SoundType.ButtonSound, 0.5f);
    }
    
    // BU TAMAMLANACAK
    public void ResetTurn() {
        // Burada Eren'in eventleri cagirmasi lazim
        
        soundManager.PlaySound(SoundType.ButtonSound, 0.5f);
    }
    
    // BU TAMAMLANACAK
    public void EndTurn() {
        // Burada Eren'in eventleri cagirmasi lazim
        
        soundManager.PlaySound(SoundType.ButtonSound, 0.5f);
    }

    public void UpdateTurnDisplay() { // TURNMANAGER GELINCE GUNCELLENECEK SU AN PSEUDO TURNMANAGER KULLANIYOR
        GameObject tempTurnUI = GameObject.Find("TurnCounter");
        TextMeshProUGUI tempText = tempTurnUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        tempText.text = (turnManager.GetCurrentTurn() + " / " + turnManager.GetMaxTurn());
        turnManager.IncreaseTurn();
    }

    public void UpdateESC_Window() {
        GameObject temp = GameObject.Find("PlayerTurnPopUp");
        
        if (temp.transform.GetChild(0).gameObject.activeInHierarchy){
            Debug.Log("1");
            soundManager.PlaySound(SoundType.EscButtonSound, 0.5f);
            temp.transform.GetChild(0).gameObject.SetActive(false);
            temp.transform.GetChild(1).gameObject.SetActive(false);
        }
        else {
            Debug.Log("2");
            soundManager.PlaySound(SoundType.EscButtonSound, 0.5f);
            temp.transform.GetChild(0).gameObject.SetActive(true);
            temp.transform.GetChild(1).gameObject.SetActive(true);
        }
        
    }

    
    class TurnManager { //SILINECEK SU ANLIK TURNMANAGER GIBI BIR SEY YAZIYORUM
        private int currentTurn;
        private int maxTurn;

        public TurnManager()
        {
            currentTurn = 1;
            maxTurn = 10;
        }
        public int GetCurrentTurn() {
            return currentTurn; 
        }
        
        public int GetMaxTurn() {
            return maxTurn; 
        }
        
        public void IncreaseTurn() {
            currentTurn++;
        }
    }
    
}
