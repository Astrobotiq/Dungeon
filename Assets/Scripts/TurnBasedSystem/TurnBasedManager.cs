using System;
using UnityEngine;
using UnityEngine.UI;

public class TurnBasedManager : Singleton<TurnBasedManager>
{
    private ITurn _currentTurn;
    
    [SerializeField]
    private int turnNumber;
    
    private int _maxTurnNumber;

    public void StartCombat(int maxTurnNumber)
    {
        turnNumber = 1;
        _maxTurnNumber = maxTurnNumber;
        InGameUITextMesh.Instance.UpdateTurnDisplay(turnNumber,_maxTurnNumber);
        NextTurn(GetComponent<EnemyCombatPositionTurn>());
    }

    public void NextTurn(ITurn nextTurn)
    {
        if (_currentTurn && _currentTurn.isLastTurn)
        {
            turnNumber++;
            Debug.Log("turn arttırması lazım");
            InGameUITextMesh.Instance.UpdateTurnDisplay(turnNumber,_maxTurnNumber);
        }
            

        if (turnNumber > _maxTurnNumber)
        {
            Debug.Log("Game is finished");
            return;
        }
            
        
        _currentTurn = nextTurn;
        Timed.Run((() => StartTurn()), 2f);
    }

    public void StartTurn()
    {
        _currentTurn.EnterTurn();
    }

    public ITurn GetCurrentTurn() => _currentTurn;

    public int getTurnNumber()
    {
        return turnNumber;
    }

    public int getMaxTurnNumber()
    {
        return _maxTurnNumber;
    }
}