using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemySpawnPosition : ITurn
{
    [SerializeField]
    private GameObject spawner;
    public override void EnterTurn()
    {
        var currentTutorialStep = TutorialManager.Instance.GetCurrentTutorialStep();
        string[] lines = currentTutorialStep.text.Split('\n', System.StringSplitOptions.RemoveEmptyEntries);
        List<GameObject> spawners = new List<GameObject>();
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            for (int j = 0; j < line.Length; j++)
            {
                char symbol = line[j];
                if (symbol.Equals('S'))
                {
                    if (GridManager.Instance.getGridFromLocation(new Vector3(i, 0, j)).GridObject == null)
                    {
                        var grid = GridManager.Instance.getGridFromLocation(new Vector3(i, 0, j));
                        var Spawner = Instantiate(spawner,
                            new Vector3(grid.transform.position.x, 0.5f, grid.transform.position.z), Quaternion.identity);
                        
                        spawners.Add(Spawner);
                
                        SoundManager.Instance.PlaySound(SoundType.InTurnEnemyInstantiateSound, 1);
                        
                        //Burada Tutorial aç
                    }
                    else
                    {
                        var grid = GridManager.Instance.getGridFromLocation(new Vector3(i+1, 0, j));
                        var Spawner = Instantiate(spawner,
                            new Vector3(grid.transform.position.x, 0.5f, grid.transform.position.z), Quaternion.identity);
                        
                        spawners.Add(Spawner);
                
                        SoundManager.Instance.PlaySound(SoundType.InTurnEnemyInstantiateSound, 1);
                        
                        //Burada tutorial aç
                    }
                }
            }
        }
        EnemyManager.Instance.SetSpawners(spawners);
        ExitTurn();
    }

    public override void ExitTurn()
    {
        TurnBasedManager.Instance.NextTurn(GetNextTurn());
    }
}