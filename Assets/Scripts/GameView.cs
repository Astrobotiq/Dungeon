using System;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    [SerializeField] Button UndoButton;

    [SerializeField] GameObject PlayerCanvas;

    [SerializeField] GameObject SkilCanvas;

    [SerializeField] Button NormalSkillBTN;

    [SerializeField] Button SpecialSkillBTN;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UndoButton.onClick.AddListener(() =>
        {
            CommandManager.Instance.Undo();
        });
    }

    void OnEnable()
    {
        CommandManager.Instance.onCommandActivated += () => UndoButton.gameObject.SetActive(true);
        CommandManager.Instance.onCommandDeactivated += () => UndoButton.gameObject.SetActive(false);
    }

    void OnDisable()
    {
        CommandManager.Instance.onCommandActivated += () => UndoButton.gameObject.SetActive(true);
        CommandManager.Instance.onCommandDeactivated += () => UndoButton.gameObject.SetActive(false);
    }

    public void OpenSkillPanel(bool isActive, Player player = null)
    {
        NormalSkillBTN.onClick.RemoveAllListeners();
        SpecialSkillBTN.onClick.RemoveAllListeners();
        if (isActive && player != null)
        {
            NormalSkillBTN.image.sprite = player.WornSkills.Normal.Skill.GUISprite;

            if (player.WornSkills.Special != null)
            {
                SpecialSkillBTN.gameObject.SetActive(true);
                //Buraya special skill'lerin şuan kullanılabilir olup olmadığına dair bir check atmalıyız
                //Eğer kullanılabilirse ozaman butonu isInteractable'ını true yapalım yapalım
            }
            
            NormalSkillBTN.onClick.AddListener((() =>
            {
                player.SetSelectedSkill(SkillType.Normal);
                NormalSkillBTN.interactable = false;
            }));
        }
        else
        {
            NormalSkillBTN.image.sprite = null;
            NormalSkillBTN.interactable = true;
            SpecialSkillBTN.interactable = false;
            SpecialSkillBTN.gameObject.SetActive(false);
        }
        SkilCanvas.gameObject.SetActive(isActive);
    }

    public void OpenPlayerPanel(bool isActive, Player player = null)
    {
        PlayerCanvas.gameObject.SetActive(isActive);
    }
}
