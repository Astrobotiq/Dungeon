using System;
using System.Collections;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField]
    GameObject PlayerGO;
    GameObject SelectedPlayer;
    
    [SerializeField,Range(0,5)]
    float offset;

    void OnEnable()
    {
        InputManager.Instance.onRightClicked += DeSelectPlayer;
    }

    void OnDisable()
    {
        InputManager.Instance.onRightClicked -= DeSelectPlayer;
    }

    void Start()
    {
        StartCoroutine(findPlayerPosition());
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

    public void SetPlayerPosition(GameObject grid)
    {
        if (SelectedPlayer == null || SelectedPlayer.GetComponent<Player>().Grid == grid)
        {
            return;
        }
        SelectedPlayer.GetComponent<Player>().SetGrid(grid, offset);
    }
    
    void DeSelectPlayer()
    {
        SelectedPlayer = null;
    }

    public void SetSelectedPlayer(GameObject Player)
    {
        if (Player == null)
        {
            SelectedPlayer = null;
            return;
        }
        
        if (SelectedPlayer == Player)
            return;

        SelectedPlayer = Player;
        var PlayerScript = SelectedPlayer.GetComponent<Player>();
        GridManager.Instance.SetSelectedGrid(PlayerScript.Grid);
    }
    
    public GameObject GetSelectedPlayer()
    {
        return SelectedPlayer;
    }
}
