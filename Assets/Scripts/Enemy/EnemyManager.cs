using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    private static List<EnemyBrain> enemies = new();
    
    public List<GameObject> enemyListForEnemyAI;

    private EnemyBrain selectedEnemy;

    public static void Subscribe(EnemyBrain enemy)
    {
        if (enemies.Contains(enemy))
        {
            return;
        }
        enemies.Add(enemy);
    }

    public static void Unsubscribe(EnemyBrain enemy)
    {
        if (!enemies.Contains(enemy))
        {
            return;
        }

        enemies.Remove(enemy);
    }

    public void SelectEnemy(EnemyBrain enemy)
    {
        if (!enemies.Contains(enemy))
        {
            Subscribe(enemy);
        }

        selectedEnemy = enemy;
    }

    public void DeselectEnemy()
    {
        if (selectedEnemy == null)
        {
            return;
        }
        
        selectedEnemy.OnEnemySelection(false);
        selectedEnemy = null;
    }
}
