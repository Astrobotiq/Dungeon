using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField]
    GameObject PlayerGO;
    GameObject SelectedPlayer;
    
    [SerializeField]
    private List<GameObject> players; 
    
    public List<GameObject> playerListForEnemyAI;
    
    [SerializeField,Range(0,5)]
    float offset;

    void OnEnable()
    {
        InputManager.Instance.onRightClicked += DeSelectPlayer;
    }

    IEnumerator findPlayerPosition()
    {
        while (GridManager.Instance.GridList.Count <= 0)
        {
            yield return null;
        }

        GameObject grid = GridManager.Instance.GetGrid();
        var PlayerScript = PlayerGO.GetComponent<Player>();
        PlayerScript.SetGridStart(grid, offset);
    }
    
    void DeSelectPlayer()
    {
        GridManager.Instance.ResetTable();
        SelectedPlayer = null;
    }

    public void SetSelectedPlayer(GameObject Player)
    {
        GridManager.Instance.ResetTable();
        SelectedPlayer = Player;
        var PlayerScript = SelectedPlayer.GetComponent<Player>();
        GridManager.Instance.SetSelectedGridFromOutside(SelectedPlayer.transform.position,
            !PlayerScript.HasTraveled && !PlayerScript.IsPlayerWebbed && PlayerScript.IsPlayerTurn , PlayerScript.range);
        EnemyManager.Instance.DeselectEnemy();
    }

    public void SetSelectedPlayerFromOutside(GameObject Player)
    {
        SelectedPlayer = Player;
    }

    public void SetNewGridForSelectedPlayer(GameObject grid)
    {
        if (SelectedPlayer == null)
            return;
        
        SelectedPlayer.GetComponent<Player>().SetGrid(grid,offset);
    }
    
    public GameObject GetSelectedPlayer()
    {
        return SelectedPlayer;
    }

    public void DeselectPlayer()
    {
        Debug.Log("Player Manager On Deselect");
        
        if (SelectedPlayer)
        {
            GridManager.Instance.ResetTable();
            SelectedPlayer.GetComponent<Player>().onDeselected();
            SelectedPlayer = null;
        }
    }

    public List<GameObject> GetPlayers() => players;
    
    public void Subscribe(Player player)
    {
        if (players.Contains(player.gameObject))
        {
            return;
        }
        players.Add(player.gameObject);
    }

    public void Unsubscribe(Player player)
    {
        if (!players.Contains(player.gameObject))
        {
            return;
        }

        players.Remove(player.gameObject);
    }
}
