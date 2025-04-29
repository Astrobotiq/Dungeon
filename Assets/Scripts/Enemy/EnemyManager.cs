using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    private static List<EnemyBrain> enemies = new();

    [SerializeField] List<GameObject> spawners = new();
    
    public List<GameObject> enemyListForEnemyAI;

    private EnemyBrain selectedEnemy;
    
    public EnemyBrain EnemyBrain
    {
        get  => selectedEnemy;
    }

    public static List<EnemyBrain> Enemies => enemies;

    public List<GameObject> Spawners => spawners;

    public void SetSpawners(List<GameObject> spawners)
    {
        this.spawners = spawners;
    }

    public void AddSpawner(GameObject spawner)
    {
        spawners.Add(spawner);
    }

    public void ResetSpawnerList()
    {
        spawners.Clear();
    }

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

        DeselectEnemy();
        selectedEnemy = enemy;
        PlayerManager.Instance.DeselectPlayer();
        GridManager.Instance.ResetTable();
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

    public static void ClearEnemyList() => enemies.Clear();
}
