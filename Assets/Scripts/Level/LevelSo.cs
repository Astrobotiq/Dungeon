using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Dungeon/Level", order = 2)]

public class LevelSO : ScriptableObject
{
    [SerializeField] 
    private TextAsset levelLayout;

    [SerializeField] 
    private List<EnemyType> enemies;

    [SerializeField] 
    private int maxTurnNumber;

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

    public int MaxTurnNumber
    {
        get
        {
            if (maxTurnNumber == 0)
            {
                maxTurnNumber = 4;
            }

            return maxTurnNumber;
        }
    }


    public List<EnemyType> GetAllEnemies() => enemies;

    public EnemyType GetRandomEnemy()
    {
        return enemies.GetRandom();
    }
}
