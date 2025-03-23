using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Dungeon/Enemy", order = 3)]
public class EnemySO : ScriptableObject
{
    public EnemyType EnemyType;

    public GameObject Enemy;

    public float Offset;
}