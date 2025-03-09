using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField]
    GameObject PlayerGO;
    GameObject SelectedPlayer;
    
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
        SelectedPlayer = null;
    }

    public void SetSelectedPlayer(GameObject Player)
    {
        SelectedPlayer = Player;
        var PlayerScript = SelectedPlayer.GetComponent<Player>();
        GridManager.Instance.SetSelectedGridFromOutside(SelectedPlayer.transform.position, !PlayerScript.HasTraveled, PlayerScript.range);
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
            SelectedPlayer.GetComponent<Player>().onDeselected();
            SelectedPlayer = null;
        }
    }
}
