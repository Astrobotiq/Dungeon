using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPlayerTurn : ITurn
{
    [SerializeField]
    private Button endTurnBTN;
    
    [SerializeField]
    private GameObject warningCanvas;
    
    [SerializeField]
    private Button passTurnAnywayBTN;
    
    [SerializeField]
    private Button cancelBTN;

    private Dictionary<Player, bool> _playerDictionary;

    private bool hasPlayerSawTutorial;
    void Start()
    {
        hasPlayerSawTutorial = false;
        endTurnBTN.onClick.AddListener((() =>
        {
            TryExitTurn();
        }));
        
        passTurnAnywayBTN.onClick.AddListener((() =>
        {
            warningCanvas.SetActive(false);
            ExitTurn();
        }));
        
        cancelBTN.onClick.AddListener((() =>
        {
            warningCanvas.SetActive(false);
        }));
    }
    public override void EnterTurn()
    {
        if (!hasPlayerSawTutorial)
        {
            TutorialManager.Instance.EnqueueTutorial(TutorialType.PlayerSelect);
            hasPlayerSawTutorial = true;
        }
        
        endTurnBTN.enabled = true;
        var playerList = PlayerManager.Instance.GetPlayers();
        _playerDictionary = new();
        foreach (var playerGO in playerList)
        {
            var player = playerGO.GetComponent<Player>();
            player.SetPlayerTurn(true);
            _playerDictionary.Add(player,false);
        }
        endTurnBTN.gameObject.SetActive(true);
    }
    
    private void TryExitTurn()
    {
        GridManager.Instance.ResetTable();
        PlayerManager.Instance.DeselectPlayer();
        foreach (var pair in _playerDictionary)
        {
            if (!pair.Value)
            {
                warningCanvas.SetActive(true);
                return;
            }
        }
        
        ExitTurn();
    }

    public override void ExitTurn()
    {
        foreach (var pair in _playerDictionary)
        {
            pair.Key.SetPlayerTurn(false);
        }
        endTurnBTN.gameObject.SetActive(false);
        
        InGameUITextMesh.Instance.ResetEnemyArrangement();
        
        TurnBasedManager.Instance.NextTurn(GetNextTurn());
    }
    
    public void SetPlayerAsPlayed(Player player)
    {
        if(!_playerDictionary.ContainsKey(player))
            return;

        _playerDictionary[player] = true;
    }
}