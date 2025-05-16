using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurn : ITurn
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

    void Start()
    {
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
        endTurnBTN.enabled = true;
        var playerList = PlayerManager.Instance.GetPlayers();
        _playerDictionary = new();
        InGameUITextMesh.Instance.OpenClosePlayerTurnIndicator();
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
        
        InGameUITextMesh.Instance.MakeEndTurnNormal();
    }

    public void SetPlayerAsPlayed(Player player)
    {
        if(!_playerDictionary.ContainsKey(player))
            return;

        _playerDictionary[player] = true;
        foreach (var item in _playerDictionary)
        {
            if (!item.Value) {
                return;
            }
        }
        InGameUITextMesh.Instance.AttractToEndTurn();
    }
}
