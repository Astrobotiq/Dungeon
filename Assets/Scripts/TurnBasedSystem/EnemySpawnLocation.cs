using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawnLocation : ITurn
{
    [SerializeField]
    private int maxSpawnNumber = 3;
    
    [SerializeField]
    private int minSpawnNumber = 1;
    
    [SerializeField]
    private float waitInterval = 1f;
    
    [SerializeField]
    private GameObject spawner;
    
    [SerializeField] 
    private SoundManager soundManager;

    public float InTurnEnemyInstantiateSoundVolume = 1f;

    private void Start()
    {
        if (soundManager == null)
        {
            Debug.Log("Soundmanager'ım yok, ben EnemySpawnLocation");
            soundManager = GameObject.FindWithTag("SoundManager").GetComponent<SoundManager>();
        }
    }

    public override void EnterTurn()
    {
        var gridList = GridManager.Instance.GridList;

        var enemyCount = EnemyManager.Enemies.Count;

        StartCoroutine(PositionFinder());
        IEnumerator PositionFinder()
        {
            var max = maxSpawnNumber - enemyCount;
            var spawnNumber = Random.Range(minSpawnNumber, max);
            var spawnedEnemyNum = 0;
            List<GameObject> spawners = new List<GameObject>();
            while (spawnedEnemyNum<spawnNumber)
            {
                var grid = gridList.GetRandom().GetRandom();

                if (grid.GetComponent<Grid>().GridObject != null)
                {
                    continue;
                }

                var Spawner = Instantiate(spawner,
                    new Vector3(grid.transform.position.x, 0.5f, grid.transform.position.z), Quaternion.identity);
                spawners.Add(Spawner);
                
                soundManager.PlaySound(SoundType.InTurnEnemyInstantiateSound, InTurnEnemyInstantiateSoundVolume);

                spawnedEnemyNum++;

                yield return new WaitForSeconds(waitInterval);
            }
            EnemyManager.Instance.SetSpawners(spawners);
            ExitTurn();
        }
    }

    public override void ExitTurn()
    {
        Debug.Log("Turn bitti");
        TurnBasedManager.Instance.NextTurn(GetNextTurn());
    }
}