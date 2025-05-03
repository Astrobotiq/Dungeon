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

    public bool hasLevelFailed = false;

    public int TurnNumber => turnNumber;

    public int MaxTurnNumber => _maxTurnNumber;

    public void Start()
    {
        if (soundManager == null)
        {
            Debug.Log("Soundmanager'ım yok, ben TurnBaseManager");
            soundManager = GameObject.FindWithTag("SoundManager").GetComponent<SoundManager>();
        }
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
        if (hasLevelFailed)
            return;
        
        if (_currentTurn &&_currentTurn.isLastTurn && turnNumber+1 > _maxTurnNumber)
        {
            Debug.Log("Game is finished");
            InGameUITextMesh.Instance.OpenWinScreen(MissionManager.Instance.GetCompletedMissionNumber());
            _currentTurn = null;
            return;
        }
        
        if (_currentTurn && _currentTurn.isLastTurn)
        {
            turnNumber++;
            InGameUITextMesh.Instance.UpdateTurnDisplay(turnNumber,_maxTurnNumber);
        }
            
        soundManager.PlaySound(SoundType.TurnSwitchSound,TurnSwitchSoundVolume);
        
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