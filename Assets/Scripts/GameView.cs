using System;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    [SerializeField] Button undoButton;

    [SerializeField] GameObject playerCanvas;

    [SerializeField] Image playerImage;

    [SerializeField] GameObject skilCanvas;

    [SerializeField] Button normalSkillBTN;

    [SerializeField] Button specialSkillBTN;

    void Start()
    {
        undoButton.onClick.AddListener(() =>
        {
            OpenSkillPanel(false);
            OpenPlayerPanel(false);
            CommandManager.Instance.Undo();
        });
    }

    void OnEnable()
    {
        if (CommandManager.Instance != null)
        {
            CommandManager.Instance.onCommandActivated += () => undoButton.gameObject.SetActive(true);
            CommandManager.Instance.onCommandDeactivated += () => undoButton.gameObject.SetActive(false);
        }
        
    }

    void OnDisable()
    {
        if (CommandManager.Instance != null)
        {
            CommandManager.Instance.onCommandActivated += () => undoButton.gameObject.SetActive(true);
            CommandManager.Instance.onCommandDeactivated += () => undoButton.gameObject.SetActive(false);
        }
        
    }
    

    public void OpenSkillPanel(bool isActive, Player player = null)
    {
        normalSkillBTN.onClick.RemoveAllListeners();
        specialSkillBTN.onClick.RemoveAllListeners();
        
        if (isActive && player != null)
        {
            normalSkillBTN.image.sprite = player.WornSkills.Normal.Skill.GUISprite;

            if (player.WornSkills.Special != null)
            {
                specialSkillBTN.gameObject.SetActive(true);
                //Buraya special skill'lerin şuan kullanılabilir olup olmadığına dair bir check atmalıyız
                //Eğer kullanılabilirse ozaman butonu isInteractable'ını true yapalım yapalım
            }
            
            normalSkillBTN.onClick.AddListener((() =>
            {
                player.SetSelectedSkill(SkillType.Normal);
                normalSkillBTN.interactable = false;
            }));
        }
        else
        {
            normalSkillBTN.image.sprite = null;
            normalSkillBTN.interactable = true;
            specialSkillBTN.interactable = false;
            specialSkillBTN.gameObject.SetActive(false);
        }
        skilCanvas.gameObject.SetActive(isActive);
    }

    public void OpenPlayerPanel(bool isActive, Player player = null)
    {
        if ( player && player.PlayerSprite)
        {
            playerImage.sprite = player.PlayerSprite;
        }
        playerCanvas.gameObject.SetActive(isActive);
    }

    public void ResetGameView()
    {
        OpenSkillPanel(false);
        OpenPlayerPanel(false);
        undoButton.gameObject.SetActive(false);
    }
}
