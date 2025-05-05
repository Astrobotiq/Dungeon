using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatPositionTurn : ITurn
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
                yield return StartCoroutine(enemy.Template());
            }
            
            ExitTurn();
        }
    }
    
    

    public override void ExitTurn()
    {
        Debug.Log("Turn bitti");
        InGameUITextMesh.Instance.UpdateEnemyArrangement(sortedEnemies);
        TurnBasedManager.Instance.NextTurn(GetNextTurn());
    }
}