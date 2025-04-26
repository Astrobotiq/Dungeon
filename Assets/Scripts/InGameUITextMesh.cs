using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class InGameUITextMesh : Singleton<InGameUITextMesh> {
    #region Lists
    
    [SerializeField] 
    private List<GameObject> players;
    
    public List<EnemyBrain> SortedEnemyBrains;
    
    #endregion


    #region General
    
    [SerializeField]
    private GameObject publicHealth;
    
    [SerializeField]
    private GameObject turnCounterGameobject;

    [SerializeField] 
    private Button UndoBTN;

    [SerializeField] 
    private GameObject youWinGameObject;
    
    [SerializeField] 
    private GameObject youLoseGameObject;

    [SerializeField] 
    private GameObject missionGameObject;
    
    [SerializeField]
    private SoundManager soundManager; //Şu an duruyor ama duruma göre SİLİNECEK duruma gelebilir

    public float WinSoundVolume = 1f;
    public float GameOverSoundVolume = 1f;
    
    #endregion
    
    //Bu duruma göre gidecek o yüzden SİLİNECEK yazıyorum
    public int MaxEnemyUICount;
    
    
    //SİLİNECEK
    private bool naber = true;
    
    private void Update()
    {
        if (naber)
        {
            List<String> temp = new List<String>();
            temp.Add("Do not take damage");
            temp.Add("Kill at least 2 wizard");
            UpdateMissionInformation(temp);
            naber = false;
        }
        
        
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
        turnCounterGameobject.gameObject.SetActive(true);
        turnCounterGameobject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (currentTurn + " / " + maxTurn);
    }

    public void OpenMissionInformation() { // SİLİNECEK geçici olarak çözmek için böyle yaptım
        missionGameObject.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void ChangeMissionInformation(int missionPlace, String missionText)
    {
        GameObject allMissionsHolder = missionGameObject.transform.GetChild(1).transform.GetChild(2).transform.GetChild(0).gameObject;
        GameObject textHolderGameobject = allMissionsHolder.transform.GetChild(missionPlace-1).transform.GetChild(1).transform.GetChild(0).gameObject;
        textHolderGameobject.GetComponent<TextMeshProUGUI>().text = missionText;
        
    }
    public void UpdateMissionInformation(List<String> missionTexts)
    {
        // OpenMissionInformation(); // Method içindeki kodu buraya taşırız geçici olarak çözmek için böyle yaptım
        
        // GameObject allMissionsHolder = missionGameObject.transform.GetChild(1).transform.GetChild(2).transform.GetChild(0).gameObject;

        if (missionTexts.Count > 2)
        {
            Debug.LogWarning("Ben 2 mission yazabiliyorum ama üçden fazla mission stringi var");
            return;
        }
        
        for (int i = 0; i < 2; i++)
        {
            // GameObject textHolderGameobject = allMissionsHolder.transform.GetChild(i).transform.GetChild(1).transform.GetChild(0).gameObject;
            // textHolderGameobject.GetComponent<TextMeshProUGUI>().text = missionTexts[i];
            
            ChangeMissionInformation(i+1, missionTexts[i]);
        }
    }

    public void OpenGameOverScreen() {
        youLoseGameObject.SetActive(true);
        
        soundManager.PlaySound(SoundType.GameOverSound, GameOverSoundVolume);
    }
    
    public void OpenWinScreen(int completedMissionCount) {
        youWinGameObject.SetActive(true);
        
        UpdateWinScreenStars(completedMissionCount);
        
        soundManager.PlaySound(SoundType.YouWinSound, WinSoundVolume);
    }
    
    public void UpdateWinScreenStars(int completedMissionCount) {
        GameObject youWinScreenStarHolderGameObject =
            youWinGameObject.transform.GetChild(2).transform.GetChild(3).gameObject;
        
        for (int i = 1; i < completedMissionCount + 1 ; i++) {
            youWinScreenStarHolderGameObject.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
    
    
}
