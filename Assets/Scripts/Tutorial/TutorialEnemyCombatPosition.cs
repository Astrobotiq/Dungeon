using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemyCombatPosition : ITurn
{
    private List<EnemyBrain> sortedEnemies;
    public override void EnterTurn()
    {
        var enemyList = EnemyManager.Enemies;
        
        enemyList.Sort((a, b) => a.InitiationPoint.CompareTo(b.InitiationPoint));
        sortedEnemies = new List<EnemyBrain>();
        sortedEnemies = enemyList;

        StartCoroutine(EnemyPositioner());

        IEnumerator EnemyPositioner()
        {
            foreach (var enemy in enemyList)
            {
                yield return StartCoroutine(enemy.TutorialTemplate());
            }
            
            ExitTurn();
        }
    }

    public override void ExitTurn()
    {
        TurnBasedManager.Instance.NextTurn(GetNextTurn());
    }
}