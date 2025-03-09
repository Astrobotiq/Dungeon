using System.Collections;
using UnityEngine;

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
    public override void EnterTurn()
    {
        var gridList = GridManager.Instance.GridList;

        StartCoroutine(PositionFinder());
        IEnumerator PositionFinder()
        {
            var spawnNumber = Random.Range(minSpawnNumber, maxSpawnNumber);
            var spawnedEnemyNum = 0;
            while (spawnedEnemyNum<spawnNumber)
            {
                var grid = gridList.GetRandom().GetRandom();

                if (grid.GetComponent<Grid>().GridObject != null)
                {
                    continue;
                }

                var Spawner = Instantiate(spawner,
                    new Vector3(grid.transform.position.x, 0.5f, grid.transform.position.z), Quaternion.identity);

                spawnedEnemyNum++;

                yield return new WaitForSeconds(waitInterval);
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