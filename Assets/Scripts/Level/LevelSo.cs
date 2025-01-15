using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Dungeon/Level", order = 2)]

public class LevelSO : ScriptableObject
{
    [SerializeField] 
    TextAsset levelLayout;

    [SerializeField] 
    List<EnemyType> enemies;

    public TextAsset LevelLayout
    {
        get
        {
            if (levelLayout == null)
            {
                Debug.Log("There is no level layout");
                return null;
            }

            return levelLayout;
        }
    }


    public List<EnemyType> GetAllEnemies() => enemies;

    public EnemyType GetRandomEnemy()
    {
        return enemies.GetRandom();
    }
}
