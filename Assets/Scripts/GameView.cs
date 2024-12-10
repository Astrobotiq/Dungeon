using System;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    [SerializeField] Button UndoButton;

    [SerializeField] GameObject PlayerCanvas;

    [SerializeField] GameObject SkilCanvas;
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
        SkilCanvas.gameObject.SetActive(isActive);
    }

    public void OpenPlayerPanel(bool isActive, Player player = null)
    {
        PlayerCanvas.gameObject.SetActive(isActive);
    }
}
