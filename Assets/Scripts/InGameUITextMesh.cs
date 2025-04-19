using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class InGameUITextMesh : Singleton<InGameUITextMesh> {
    
    [SerializeField] 
    private List<GameObject> players;

    private GameObject publicHealth;

    [SerializeField] 
    private SoundManager soundManager;
    
    public GameObject TurnCounterGameobject;

    [SerializeField] private Button UndoBTN;

    public List<EnemyBrain> SortedEnemyBrains;

    public int MaxEnemyUICount;

    public void Awake() {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        publicHealth = GameObject.Find("PublicHealth");
    }
    
    void OnEnable()
    {
        CommandManager.Instance.onCommandActivated += () => UndoBTN.gameObject.SetActive(true);
        CommandManager.Instance.onCommandDeactivated += () => UndoBTN.gameObject.SetActive(false);
    }

    void OnDisable()
    {
        CommandManager.Instance.onCommandActivated += () => UndoBTN.gameObject.SetActive(true);
        CommandManager.Instance.onCommandDeactivated += () => UndoBTN.gameObject.SetActive(false);
    }

    public void EnemyHoverEnter(int input) {
        //Debug.Log("ben hover giriyorum " + SortedEnemyBrains[input - 1].gameObject.name);
        SortedEnemyBrains[input - 1].UIRefHoverEnter();
    }
    
    public void EnemyHoverExit(int input) {
        //Debug.Log("ben hover çıkıyorum " + SortedEnemyBrains[input - 1].gameObject.name);
        SortedEnemyBrains[input - 1].UIRefHoverExit();
    }

    public void updatePublicBar() {
        if (! (publicHealth.transform.GetChild(0).gameObject.activeInHierarchy)) {
            publicHealth.transform.GetChild(0).gameObject.SetActive(true);
            publicHealth.transform.GetChild(1).gameObject.SetActive(true);
        }
        
        Slider publicBarSlider = publicHealth.transform.GetChild(0).GetComponent<Slider>();
        //Debug.Log("Public health döndü değer " + VillageManager.Instance.getHealthPercentage());
        publicBarSlider.value = VillageManager.Instance.getHealthPercentage();
        
        if (publicBarSlider.value < 0) {
            publicBarSlider.value = 0;
        }
        else if (publicBarSlider.value > 100) {
            publicBarSlider.value = 100;
        }
    }

    public void UpdatePlayerBars() {
        players = PlayerManager.Instance.GetPlayers();
        
        for (int i = 0; i < players.Count; i++) {
            GameObject temp = GameObject.Find("PlayerHealth_" + (i+1) );
            temp.transform.GetChild(0).gameObject.SetActive(true); // PlayerHealth slider active eder
            temp.transform.GetChild(1).gameObject.SetActive(true); // PlayerHealth iconFrame active eder
            
            UpdatePlayerImage(temp,players[i]);
            UpdatePlayerHP(temp, players[i]);
        }
    }
    
    private void UpdatePlayerImage(GameObject PlayerBar, GameObject player) {
        Image tempImage = PlayerBar.transform.GetChild(1).GetComponent<Image>();
        Player playerComponent = player.GetComponent<Player>();
        
        if (playerComponent.PlayerSprite) {
            tempImage.sprite = player.GetComponent<Player>().PlayerSprite;
        }
        else {
            Debug.Log("PlayerImage bulamadim su playerBar icin" + PlayerBar.name);
        }
        
    }

    private void UpdatePlayerHP(GameObject PlayerBar, GameObject player) {
        Slider tempSlider = PlayerBar.transform.GetChild(0).GetComponent<Slider>();
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        //Debug.Log("Slider value " + tempSlider.value + " player health " + playerHealth.getHealthPercentage() );
        
        //Debug.Log("Slider value " + tempSlider.value + " player health " + playerHealth.getHealthPercentage() );
        tempSlider.value = playerHealth.getHealthPercentage();
        
        if (tempSlider.value < 0) {
            tempSlider.value = 0;
        }
        else if (tempSlider.value > 100) {
            tempSlider.value = 100;
        }
    }
    
    public void UpdateEnemyArrangement(List<EnemyBrain> input)
    {
        SortedEnemyBrains = new List<EnemyBrain>();
        SortedEnemyBrains = input;
        
        for (int i = 0; i < MaxEnemyUICount; i++) {
            GameObject temp = GameObject.Find("Enemy_" + (i+1) );
            temp.transform.GetChild(0).gameObject.SetActive(true); // EnemyProfıle active eder
            temp.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = input[i].enemyPortrait;
            //temp.transform.GetChild(1).gameObject.SetActive(true); // Enemy Level (bızım ıcın oynama sırası) active eder. DND initiation 
        }
    }

    public void UpdateTurnDisplay(int currentTurn, int maxTurn) { // TURNMANAGER GELINCE GUNCELLENECEK SU AN PSEUDO TURNMANAGER KULLANIYOR
        TurnCounterGameobject.gameObject.SetActive(true);
        TurnCounterGameobject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (currentTurn + " / " + maxTurn);
    }
}
