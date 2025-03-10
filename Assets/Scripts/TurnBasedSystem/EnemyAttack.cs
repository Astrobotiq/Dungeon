using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyAttack : ITurn
{
    [SerializeField]
    private float waitInterval = 1f;
    
    
    public override void EnterTurn()
    {
        var enemies = EnemyManager.Enemies;
        
        enemies.Sort((a, b) => a.InitiationPoint.CompareTo(b.InitiationPoint));
        
        StartCoroutine(EnemyAttack());

        IEnumerator EnemyAttack()
        {
            foreach (var enemy in enemies)
            {
                enemy.Attack();

                yield return new WaitForSeconds(waitInterval);
            }
            
            ExitTurn();
        }
        
    }

    public override void ExitTurn()
    {
        TurnBasedManager.Instance.NextTurn(GetNextTurn());
    }
}

/*
 * public override void EnterTurn()
    {
        var spawners = EnemyManager.Instance.Spawners;

        IEnumerator SpawnEnemies()
        {
            foreach (var enemy in spawners)
            {
                while (true)
                {
                    yield return null;
                }
            }
        }
    }
 */