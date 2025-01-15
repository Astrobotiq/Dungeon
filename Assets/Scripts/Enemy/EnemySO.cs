using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Dungeon/Enemy", order = 3)]
public class EnemySO : ScriptableObject
{
    [SerializeField] 
    uint EnemyID;
    
    public EnemyType EnemyType { get;}

    public GameObject Enemy;
}