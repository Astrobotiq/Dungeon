using System;
using UnityEngine;
using UnityEngine.UI;

public class TurnBasedManager : Singleton<TurnBasedManager>
{
    private ITurn _currentTurn;
    void Start()
    {
        NextTurn(GetComponent<EnemyCombatPositionTurn>());
    }

    public void NextTurn(ITurn nextTurn)
    {
        _currentTurn = nextTurn;
        Timed.Run((() => StartTurn()), 2f);
    }

    public void StartTurn()
    {
        _currentTurn.EnterTurn();
    }

    public ITurn GetCurrentTurn() => _currentTurn;
}