using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemySpawnTurn : ITurn
{
    [SerializeField]
    private float waitInterval = 1f;
    public override void EnterTurn()
    {
        var spawners = EnemyManager.Instance.Spawners;

        StartCoroutine(SpawnEnemies());

        IEnumerator SpawnEnemies()
        {
            foreach (var location in spawners)
            {
                var pos = new Vector3(location.transform.position.x,1.4f, location.transform.position.z);
                var enemy = LevelManager.Instance.EnemyList.GetRandom();
                Instantiate(enemy, pos, quaternion.identity);
                Destroy(location);
                yield return new WaitForSeconds(waitInterval);
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