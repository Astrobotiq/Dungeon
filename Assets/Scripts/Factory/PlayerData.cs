using UnityEngine;
[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData")]
public class PlayerData : ScriptableObject
{
    public PlayerType PlayerType;
    public GameObject PlayerPrefab;
    public float Offset;
}

public enum PlayerType{
    Paladin,
    Wizard,
    Rouge,
    None
}