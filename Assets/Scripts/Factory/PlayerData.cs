using System;
using UnityEngine;
[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData")]
public class PlayerData : BaseData
{
    public PlayerType PlayerType;
    public override Enum GetTypeEnum() => PlayerType;
}

public enum PlayerType{
    Paladin,
    Wizard,
    Rouge,
    None
}