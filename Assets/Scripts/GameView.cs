using System;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    [SerializeField] Button UndoButton;
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
