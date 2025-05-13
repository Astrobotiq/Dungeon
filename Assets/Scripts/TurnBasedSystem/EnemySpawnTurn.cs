using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemySpawnTurn : ITurn
{
    [SerializeField]
    private float waitInterval = 1f;

    [SerializeField] 
    private ArmController armController;

    [SerializeField] 
    private int damageAmount = 1;
    
    [SerializeField]
    private EnemyFactory enemyFactory;
    
    public override void EnterTurn()
    {
        var spawners = EnemyManager.Instance.Spawners;

        if (spawners.Count == 0)
        {
            Debug.Log("Spawner olmadığı için spawn turu geçildi.");
            ExitTurn();
            return;
        }

        StartCoroutine(SpawnEnemies());

        IEnumerator SpawnEnemies()
        {
            foreach (var location in spawners)
            {
                if (!TurnBasedManager.Instance.hasLevelFailed)
                {
                    var armPos = ArmController.Instance.GetPosition;
                    var enemy = enemyFactory.BuildRandom(armPos,quaternion.identity);
                    var enemyGO = enemy.Item1;
                    enemyGO.transform.position = armPos;
                    var enemyOffset = enemy.Item2; 
                    var pos = new Vector3(location.transform.position.x,enemyOffset, location.transform.position.z);
                    ArmController.Instance.EnqueueStartInstantiate(enemyGO,pos,waitInterval, damageAmount);
                    Destroy(location);
                    yield return new WaitForSeconds(2*waitInterval+3f);
                }
            }
            EnemyManager.Instance.ResetSpawnerList();
            ExitTurn();
        }
    }

    public override void ExitTurn()
    {
        if (TutorialManager.Instance.isInTutorialLevel)
        {
            TutorialManager.Instance.BuildTutorialLevel();
            return;
        }
        TurnBasedManager.Instance.NextTurn(GetNextTurn());
    }
}