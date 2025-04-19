using System;
using UnityEngine;
using UnityEngine.UI;

public class TurnBasedManager : Singleton<TurnBasedManager>
{
    private ITurn _currentTurn;
    
    [SerializeField]
    private int turnNumber;
    
    [SerializeField]
    private int _maxTurnNumber;
    
    [SerializeField] 
    private SoundManager soundManager;

    public float TurnSwitchSoundVolume = 1f;

    public void Start()
    {
        soundManager = GameObject.FindWithTag("SoundManager").GetComponent<SoundManager>();
    }

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
            InGameUITextMesh.Instance.UpdateTurnDisplay(turnNumber,_maxTurnNumber);
        }
            

        if (turnNumber > _maxTurnNumber)
        {
            Debug.Log("Game is finished");
            //TODO : Burada bir yerde Level bitti UI'ı açılmalı
            return;
        }
            
        
        _currentTurn = nextTurn;
        Timed.Run((() => StartTurn()), 2f);
    }

    public void StartTurn()
    {
        _currentTurn.EnterTurn();
        
        soundManager.PlaySound(SoundType.TurnSwitchSound,TurnSwitchSoundVolume);
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