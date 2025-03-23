using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDB", menuName = "PlayerDB")]
public class PlayerDB : ScriptableObject
{
    [SerializeField]
    List<PlayerData> PlayerDataList;

    public (GameObject,float) GetPlayer(PlayerType playerType)
    {
        foreach (var playerData in PlayerDataList)
        {
            if (playerData.PlayerType == playerType)
            {
                return (playerData.PlayerPrefab,playerData.Offset);
            }
        }

        return (null,0);
    }
}