using System;
using UnityEngine;
using UnityEngine.UI;

public class TurnBasedManager : Singleton<TurnBasedManager>
{
    private ITurn _currentTurn;
    
    [SerializeField]
    private Button button; 
    void Start()
    {
        _currentTurn = GetComponent<EnemyCombatPositionTurn>();
    }

    public void NextTurn(ITurn nextTurn)
    {
        _currentTurn = nextTurn;
        button.interactable = true;
    }

    public void StartTurn()
    {
        _currentTurn.EnterTurn();
        button.interactable = false;
    }

    public ITurn GetCurrentTurn() => _currentTurn;
}