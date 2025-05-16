using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class InGameUITextMesh : Singleton<InGameUITextMesh> {
    #region Lists
    
    [SerializeField] 
    private List<GameObject> players;
    
    public List<EnemyBrain> SortedEnemyBrains;

    public List<GameObject> EnemyUIGameobjects;

    private List<GameObject> EnemyUIRelatedGameobjectHolder;

    public List<GameObject> PlayerUIGameobjects;
     
    #endregion


    #region General
    
    [SerializeField]
    private GameObject InGameUICanvas;
    
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
    private GameObject LevelCanvas;

    [SerializeField] 
    private GameObject EndTurnButton;

    private ColorBlock EndTurnBaseColorBlock;

    [SerializeField] 
    private GameObject PlayerTurnIndicator;

    [SerializeField] 
    private float PlayerTurnIndicatorTimeOnScreen = 1f;
    
    [SerializeField]
    private SoundManager soundManager; //Şu an duruyor ama duruma göre SİLİNECEK duruma gelebilir

    public float WinSoundVolume = 1f;
    public float GameOverSoundVolume = 1f;
    
    #endregion
    
    //Bu duruma göre gidecek o yüzden SİLİNECEK yazıyorum
    public int MaxEnemyUICount;

    public bool continueToAttract = true;

    public void Start()
    {
        if (soundManager == null)
        {
            Debug.Log("Soundmanager'ım yok, ben InGameUITextMesh");
            soundManager = GameObject.FindWithTag("SoundManager").GetComponent<SoundManager>();
        }
    }

    void OnEnable()
    {
        if (CommandManager.Instance != null)
        {
            CommandManager.Instance.onCommandActivated += () => UndoBTN.gameObject.SetActive(true);
            CommandManager.Instance.onCommandDeactivated += () => UndoBTN.gameObject.SetActive(false);
        }
    }

    void OnDisable()
    {
        if (CommandManager.Instance != null)
        {
            CommandManager.Instance.onCommandActivated += () => UndoBTN.gameObject.SetActive(true);
            CommandManager.Instance.onCommandDeactivated += () => UndoBTN.gameObject.SetActive(false);
        }
    }

    public void OpenInGameUICanvas()
    {
        InGameUICanvas.gameObject.SetActive(true);
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

    // Bu metodun kullandigi PlayerUIGameobjects listesine PaladinUI -> JesterUI-> WizardUI seklinde oldugundan emin ol yoksa yanlis calisir su an baya stricly typed bir method
    public void UpdatePlayerBars() 
    {
        players = PlayerManager.Instance.GetPlayers();
        
        for (int i = 0; i < players.Count; i++)
        {
            GameObject temp = null;

            if (players[i].GetComponent<Player>().CharacterType == CharacterType.Paladin) {
                temp = PlayerUIGameobjects[0];
                temp.transform.GetChild(0).gameObject.SetActive(true); // PlayerHealth slider active eder
                temp.transform.GetChild(1).gameObject.SetActive(true); // PlayerHealth iconFrame active eder
            }
            else if (players[i].GetComponent<Player>().CharacterType == CharacterType.Jester) {
                temp = PlayerUIGameobjects[1];
                temp.transform.GetChild(0).gameObject.SetActive(true); // PlayerHealth slider active eder
                temp.transform.GetChild(1).gameObject.SetActive(true); // PlayerHealth iconFrame active eder
            }
            else if (players[i].GetComponent<Player>().CharacterType == CharacterType.Wizard) {
                temp = PlayerUIGameobjects[2];
                temp.transform.GetChild(0).gameObject.SetActive(true); // PlayerHealth slider active eder
                temp.transform.GetChild(1).gameObject.SetActive(true); // PlayerHealth iconFrame active eder
            }
            else {
                Debug.Log("Burada bi sıkıntı var gel bak");
            }
            
            UpdatePlayerImage(temp,players[i]);
            UpdatePlayerHP(temp, players[i]);
        }
    }

    /*public void UpdatePlayerBars() {
        players = PlayerManager.Instance.GetPlayers();
        
        for (int i = 0; i < players.Count; i++) {
            GameObject temp = GameObject.Find("PlayerHealth_" + (i+1) );
            temp.transform.GetChild(0).gameObject.SetActive(true); // PlayerHealth slider active eder
            temp.transform.GetChild(1).gameObject.SetActive(true); // PlayerHealth iconFrame active eder
            
            UpdatePlayerImage(temp,players[i]);
            UpdatePlayerHP(temp, players[i]);
        }
    }*/
    
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
        
        Debug.Log("Slider value " + tempSlider.value + " player health " + playerHealth.getHealthPercentage() + "player name " + player.name);
        tempSlider.value = playerHealth.getHealthPercentage();
        
        if (tempSlider.value < 0) {
            tempSlider.value = 0;
        }
        else if (tempSlider.value > 100) {
            tempSlider.value = 100;
        }
        
    }

    public void UpdateSpecificPlayer(GameObject player)
    {
        GameObject temp = null;
        
        if (player.GetComponent<Player>().CharacterType == CharacterType.Paladin) {
            temp = PlayerUIGameobjects[0];
            temp.transform.GetChild(0).gameObject.SetActive(true); // PlayerHealth slider active eder
            temp.transform.GetChild(1).gameObject.SetActive(true); // PlayerHealth iconFrame active eder
        }
        else if (player.GetComponent<Player>().CharacterType == CharacterType.Jester) {
            temp = PlayerUIGameobjects[1];
            temp.transform.GetChild(0).gameObject.SetActive(true); // PlayerHealth slider active eder
            temp.transform.GetChild(1).gameObject.SetActive(true); // PlayerHealth iconFrame active eder
        }
        else if (player.GetComponent<Player>().CharacterType == CharacterType.Wizard) {
            temp = PlayerUIGameobjects[2];
            temp.transform.GetChild(0).gameObject.SetActive(true); // PlayerHealth slider active eder
            temp.transform.GetChild(1).gameObject.SetActive(true); // PlayerHealth iconFrame active eder
        }
        else {
            Debug.Log("Burada bi sıkıntı var gel bak");
        }
            
        UpdatePlayerImage(temp,player);
        UpdatePlayerHP(temp, player);
    }
    
    public void UpdateEnemyArrangement(List<EnemyBrain> input)
    {
        SortedEnemyBrains = new List<EnemyBrain>();
        SortedEnemyBrains = input;
        
        if (SortedEnemyBrains.Count >= MaxEnemyUICount) {
            EnemyUIRelatedGameobjectHolder = new List<GameObject>();
            for (int i = 0; i < MaxEnemyUICount; i++) {
                GameObject temp = EnemyUIGameobjects[i];
                EnemyUIRelatedGameobjectHolder.Add(SortedEnemyBrains[i].gameObject);
                temp.transform.GetChild(0).gameObject.SetActive(true); // EnemyProfıle active eder 
                temp.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = input[i].enemyPortrait;
                //temp.transform.GetChild(1).gameObject.SetActive(true); // Enemy Level (bızım ıcın oynama sırası) active eder. DND initiation 
            }
        }
        else
        {
            EnemyUIRelatedGameobjectHolder = new List<GameObject>();

            int dif = Math.Abs(SortedEnemyBrains.Count - MaxEnemyUICount);
            
            for (int i = 0; i < dif; i++) {
                EnemyUIRelatedGameobjectHolder.Add(null);
            }

            foreach (var enemyBrain in SortedEnemyBrains) {
                EnemyUIRelatedGameobjectHolder.Add(enemyBrain.gameObject);
            }
            
            for (int i = dif; i < MaxEnemyUICount ; i++) {
                GameObject temp = EnemyUIGameobjects[i];
                temp.transform.GetChild(0).gameObject.SetActive(true); // EnemyProfıle active eder 
                temp.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = input[i-dif].enemyPortrait;
                //temp.transform.GetChild(1).gameObject.SetActive(true); // Enemy Level (bızım ıcın oynama sırası) active eder. DND initiation
            }
        }
    }

    public void ResetEnemyArrangement()
    {
        for (int i = 0; i < MaxEnemyUICount; i++) {
            GameObject temp = GameObject.Find("Enemy_" + (i+1) );
            temp.transform.GetChild(0).gameObject.SetActive(false); // EnemyProfıle active eder
            //temp.transform.GetChild(1).gameObject.SetActive(true); // Enemy Level (bızım ıcın oynama sırası) active eder. DND initiation 
        }
        
        EnemyUIRelatedGameobjectHolder.Clear();
    }
    
    public void EnemyHoverEnter(int input) {
        //Debug.Log("ben hover giriyorum " + SortedEnemyBrains[input - 1].gameObject.name);
        EnemyUIRelatedGameobjectHolder[input - 1].GetComponent<EnemyBrain>().UIRefHoverEnter();
    }
    
    public void EnemyHoverExit(int input) {
        //Debug.Log("ben hover çıkıyorum " + SortedEnemyBrains[input - 1].gameObject.name);
        EnemyUIRelatedGameobjectHolder[input - 1].GetComponent<EnemyBrain>().UIRefHoverExit();
    }

    public void UpdateTurnDisplay(int currentTurn, int maxTurn) { // TURNMANAGER GELINCE GUNCELLENECEK SU AN PSEUDO TURNMANAGER KULLANIYOR
        turnCounterGameobject.gameObject.SetActive(true);
        turnCounterGameobject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (currentTurn + " / " + maxTurn);
    }

    public void OpenClosePlayerTurnIndicator()
    {
        GameObject tempRef = PlayerTurnIndicator.transform.GetChild(0).gameObject;
        
        tempRef.SetActive(true);

        Timed.Run((() => tempRef.SetActive(false)),PlayerTurnIndicatorTimeOnScreen);
    }

    public void OpenMissionInformation() { // SİLİNECEK geçici olarak çözmek için böyle yaptım
        missionGameObject.transform.GetChild(1).gameObject.SetActive(true);
    }
    
    public void UpdateMissionInformation(List<MissionParameter> missionTexts)
    {
        OpenMissionInformation(); // Method içindeki kodu buraya taşırız geçici olarak çözmek için böyle yaptım

        var missionInfo = MissionManager.Instance.GetMissionInfo(missionTexts);
        
        // GameObject allMissionsHolder = missionGameObject.transform.GetChild(1).transform.GetChild(2).transform.GetChild(0).gameObject;

        if (missionInfo.Count > 2)
        {
            Debug.LogWarning("Ben 2 mission yazabiliyorum ama üçden fazla mission stringi var");
            return;
        }
        
        for (int i = 0; i < missionInfo.Count; i++)
        {
            // GameObject textHolderGameobject = allMissionsHolder.transform.GetChild(i).transform.GetChild(1).transform.GetChild(0).gameObject;
            // textHolderGameobject.GetComponent<TextMeshProUGUI>().text = missionTexts[i];
            
            ChangeMissionInformation(i+1, missionInfo[i]);
        }
    }

    public void ChangeMissionInformation(int missionPlace, String missionText)
    {
        GameObject allMissionsHolder = missionGameObject.transform.GetChild(1).transform.GetChild(2).transform.GetChild(0).gameObject;
        GameObject textHolderGameobject = allMissionsHolder.transform.GetChild(missionPlace-1).transform.GetChild(1).transform.GetChild(0).gameObject;
        textHolderGameobject.GetComponent<TextMeshProUGUI>().text = missionText;
        
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
    
    public void AttractToEndTurn()
    {
        EndTurnBaseColorBlock = EndTurnButton.GetComponent<Button>().colors;
        continueToAttract = true;
        StartCoroutine(OpenCloseEndTurn());
    }
    IEnumerator OpenCloseEndTurn()
    {
        ColorBlock firstColorBlock = EndTurnButton.GetComponent<Button>().colors;

        // This part lover the alpha value so it becomes nearly invisible
        ColorBlock tempcolorBlock = firstColorBlock;
        Color changedColor = new Color(tempcolorBlock.normalColor.r, tempcolorBlock.normalColor.g, tempcolorBlock.normalColor.b, tempcolorBlock.normalColor.a - 0.5f);
        tempcolorBlock.normalColor = changedColor;

        while (continueToAttract)
        {
            EndTurnButton.GetComponent<Button>().colors = tempcolorBlock;
            EndTurnButton.transform.GetChild(0).gameObject.SetActive(false); // Deactivate Text
            yield return new WaitForSeconds(1f);

            EndTurnButton.GetComponent<Button>().colors = firstColorBlock;
            EndTurnButton.transform.GetChild(0).gameObject.SetActive(true); // Activate Text
            yield return new WaitForSeconds(1f);
        }
        
        EndTurnButton.GetComponent<Button>().colors = EndTurnBaseColorBlock;
        yield return null;
    }
    public void MakeEndTurnNormal()
    {
        continueToAttract = false;
    }
    
    public void UpdateWinScreenStars(int completedMissionCount) {
        GameObject youWinScreenStarHolderGameObject =
            youWinGameObject.transform.GetChild(2).transform.GetChild(3).gameObject;
        
        for (int i = 1; i < completedMissionCount + 1 ; i++) {
            youWinScreenStarHolderGameObject.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void LevelChange(Button button)
    {
        button.enabled = false;
        
        LevelCanvas.SetActive(false);
        
        CameraManager.Instance.ChangeCameraForLevel();
    }
    public void OpenLevelSelection()
    {
        LevelCanvas.SetActive(true);
    }
    
    // Call this method to quit the game
    public void QuitGame()
    {
        // If we are running in a standalone build
        Application.Quit();

        // If we are in the Unity editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    
}
