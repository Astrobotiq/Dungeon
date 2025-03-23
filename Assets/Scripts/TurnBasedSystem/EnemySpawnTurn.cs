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
    private int damageAmount = 3;
    
    public override void EnterTurn()
    {
        var spawners = EnemyManager.Instance.Spawners;

        StartCoroutine(SpawnEnemies());

        IEnumerator SpawnEnemies()
        {
            foreach (var location in spawners)
            {
                var armPos = ArmController.Instance.GetPosition;
                var pos = new Vector3(location.transform.position.x,1.4f, location.transform.position.z);
                var enemy = EnemyFactory.Instance.BuildRandom(armPos,quaternion.identity);
                ArmController.Instance.StartInstantiate(enemy,pos,waitInterval, damageAmount);
                Destroy(location);
                yield return new WaitForSeconds(2*waitInterval+1f);
            }
            EnemyManager.Instance.ResetSpawnerList();
            ExitTurn();
        }
    }

    public override void ExitTurn()
    {
        TurnBasedManager.Instance.NextTurn(GetNextTurn());
    }
}