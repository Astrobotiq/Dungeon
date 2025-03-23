using System;
using UnityEngine;

public class PlayerFactory : Singleton<PlayerFactory>
{
    [SerializeField]
    private PlayerDB playerDB;
    
    public GameObject Build(string name, Vector3 position, Quaternion quaternion)
    {
        PlayerType player = PlayerType.None;
        switch (name)
        {
            case FactoryParameters.Paladin:
                player = PlayerType.Paladin;
                break;
            case FactoryParameters.Wizard:
                player = PlayerType.Wizard;
                break;
            case FactoryParameters.Rouge:
                player = PlayerType.Rouge;
                break;
        }

        if (player == PlayerType.None)
        {
            Debug.LogWarning("Player type düzgün verilmemiş");
            return null;
        }

        var Player = playerDB.GetPlayer(player);
        var PlayerGO = Player.Item1;
        var offset = Player.Item2;
        var pos = new Vector3(position.x,offset,position.z);
        return Instantiate(PlayerGO, pos, quaternion);
    }

    public GameObject BuildRandom(Vector3 position, Quaternion quaternion)
    {
        Debug.LogWarning("Playerlar random şekilde build edilemez");
        return null;
    }
}

