using System;
using System.Collections;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField]
    GameObject PlayerGO;
    Player PlayerScript;
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
        PlayerScript = PlayerGO.GetComponent<Player>();
        StartCoroutine(findPlayerPosition());
    }

    IEnumerator findPlayerPosition()
    {
        while (GridManager.Instance.GridList.Count <= 0)
        {
            yield return null;
        }

        GameObject grid = GridManager.Instance.GetGrid();
        PlayerScript.SetGridStart(grid, offset);
    }

    public void SetPlayerPosition(GameObject grid)
    {
        if (SelectedPlayer != PlayerScript.gameObject)
        {
            return;
        }
        PlayerScript.SetGrid(grid, offset);
    }

    void DeSelectPlayer()
    {
        SelectedPlayer = null;
    }

    public void SetSelectedPlayer(GameObject Player)
    {
        if (SelectedPlayer == Player)
        {
            return;
        }

        SelectedPlayer = Player;
        PlayerScript = SelectedPlayer.GetComponent<Player>();
    }
}
