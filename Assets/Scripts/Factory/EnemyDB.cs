using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDB", menuName = "EnemyDB")]
public class EnemyDB : ScriptableObject
{
    public List<EnemySO> EnemyList;

    public (GameObject,float) GetEnemyByType(EnemyType enemyType)
    {
        foreach (var EnemyData in EnemyList)
        {
            if (EnemyData.EnemyType == enemyType)
            {
                var enemy = EnemyList.GetRandom();
                return (enemy.Enemy,enemy.Offset);
            }
        }

        return (null,0f);
    }

    public (GameObject,float) GetRandomEnemy()
    {
        var enemy = EnemyList.GetRandom();
        return (enemy.Enemy,enemy.Offset);
    }
}