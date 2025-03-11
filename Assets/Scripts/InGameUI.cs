using System;
using System.Collections.Generic;
using MoreMountains.FeedbacksForThirdParty;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class InGameUI : MonoBehaviour
{
    private UIDocument document;

    private List<Button> buttons;
    public AudioSource buttonSound;
    
    [SerializeField] [Range(0, 100)] 
    private float statueHeealthFloat = 10;
    private ProgressBar statue;
    
    
    
    //İLERİDE SİLİNECEK Player için duruyor
    private List<VisualElement> players;
    private List<GameObject> playerGameObjects;
    public GameObject player;
    public bool playerCheck;
    private VisualElement playerContainer;
    
    private PlayerManager playerManager;
    
    //İLERİDE SİLİNECEK Enemy için duruyor
    private List<VisualElement> enemies;
    private List<GameObject> enemyGameObjects;
    public GameObject enemy;
    public bool enemyCheck;
    private VisualElement enemyContainer;
    
    public void Awake() {
        document = GetComponent<UIDocument>();
        
        VisualElement buttonContainer = document.rootVisualElement.Q("Buttons");
        buttons = buttonContainer.Query<Button>().ToList();
        foreach (Button button in buttons) {
            button.RegisterCallback<ClickEvent>(onAllButtonClick);
        }

        VisualElement statueContainer = document.rootVisualElement.Q("StatueBlock");
        statue = statueContainer.Q<ProgressBar>();
        
        playerContainer = document.rootVisualElement.Q("CharacterBlock");

        enemyContainer = document.rootVisualElement.Q("EnemyBlock");
    }

    public void Update() {
        statue.value = statueHeealthFloat;

        /*if (playerCheck) {
            playerGameObjects = new List<GameObject>();
            playerGameObjects.Add(player);
            for (int i = 0; i < playerGameObjects.Count; i++) {
                String temp = "Character_" + (i+1).ToString();
                VisualElement temp_element = playerContainer.Q(temp);
                temp_element.style.display = DisplayStyle.Flex;
                float playerHealth = playerGameObjects[i].GetComponent<PlayerHealth>().getHealthPercentage();
                temp_element.Q<ProgressBar>().value = playerHealth;
            }
            playerCheck = false;
        }*/
        
        if (playerCheck) {
            playerGameObjects = new List<GameObject>();
            playerGameObjects.Add(player);
            for (int i = 0; i < playerGameObjects.Count; i++) {
                String temp = "Character_" + (i+1).ToString();
                VisualElement temp_element = playerContainer.Q(temp);
                temp_element.style.display = DisplayStyle.Flex;
                int playerHealth = playerGameObjects[i].GetComponent<PlayerHealth>().getHealth() / 2;

                VisualElement HealthBoxContainer = temp_element.Q("HealthBoxContainer");
                HealthBoxContainer.style.display = DisplayStyle.Flex;
                List<Label> temp_list = HealthBoxContainer.Query<Label>().ToList();

                for (int j = 0; j < temp_list.Count; j++){
                    if (j < Mathf.Floor(playerHealth)) {
                        temp_list[j].style.backgroundColor = new Color(0.3058824f,0.8666667f,0.08627451f,1f);
                    }
                    else {
                        temp_list[j].style.backgroundColor = new Color(0.282353f,1f,0.08627451f,0.2509804f);
                    }
                }

            }
            playerCheck = false;
        }

        if (enemyCheck) {
            enemyGameObjects = new List<GameObject>();
            enemyGameObjects.Add(enemy);
            for (int i = 0; i < enemyGameObjects.Count; i++) {
                String temp = "Enemy_" + (i+1).ToString();
                Debug.Log(temp);
                VisualElement temp_element = enemyContainer.Q(temp);
                temp_element.style.display = DisplayStyle.Flex;
            }
            enemyCheck = false;
        }
        
    }

    private void OnDisable() {
        foreach (Button button in buttons) {
            button.UnregisterCallback<ClickEvent>(onAllButtonClick);
        }

        for (int i = 0; i < enemies.Count; i++) {
            String temp = "Enemy_" + (i+1).ToString();
            Debug.Log(temp);
            VisualElement temp_element = enemies[i].Q(temp);
            temp_element.style.display = DisplayStyle.None;
        }
    }

    public void OnEnable()
    {
        for (int i = 0; i < 3; i++) {
            String temp = "Enemy_" + (i+1).ToString();
            Debug.Log(temp);
            VisualElement temp_element = document.rootVisualElement.Q("EnemyBlock").Q(temp);
            temp_element.RegisterCallback<MouseEnterEvent>(evt => temp_element.Q(temp + "_Info").style.display = DisplayStyle.Flex);
            temp_element.RegisterCallback<MouseLeaveEvent>(evt => temp_element.Q(temp + "_Info").style.display = DisplayStyle.None);
        }
    }

    private void onAllButtonClick(ClickEvent evt) {
        Debug.Log("button çalıştı");
        buttonSound.Play();
    }
}
