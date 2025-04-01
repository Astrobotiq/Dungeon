using System;
using UnityEngine;

public class PlayerFactory : BaseFactory<PlayerDB, PlayerType>
{
    public override (GameObject,float) BuildRandom(Vector3 position, Quaternion rotation)
    {
        Debug.LogWarning("Players cannot be randomly built.");
        return (null,0);
    }
}

