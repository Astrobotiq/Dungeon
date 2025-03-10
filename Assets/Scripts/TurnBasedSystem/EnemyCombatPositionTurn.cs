using System.Collections;
using UnityEngine;

public class EnemyCombatPositionTurn : ITurn
{
    public override void EnterTurn()
    {
        var enemyList = EnemyManager.Enemies;
        
        enemyList.Sort((a, b) => a.InitiationPoint.CompareTo(b.InitiationPoint));

        StartCoroutine(EnemyPositioner());

        IEnumerator EnemyPositioner()
        {
            foreach (var enemy in enemyList)
            {
                StartCoroutine(enemy.Template());

                yield return new WaitForSeconds(5f);
            }
            
            ExitTurn();
        }
    }
    
    

    public override void ExitTurn()
    {
        Debug.Log("Turn bitti");
        TurnBasedManager.Instance.NextTurn(GetNextTurn());
    }
}