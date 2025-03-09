using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    //Bu class'ı bir amaçla açtım ama sonradan bunu başka bir yerde de yapabileceğimi fark ettim. Şimdilik burada dursun sonra birşeyler eklenebilir.
    [SerializeField] LevelSO currentLevel;

    [SerializeField] GameObject Player;
    [SerializeField] List<GameObject> EnemyList;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentLevel = LevelDB.Instance.GetRandomLevel();
        StartCoroutine(LevelDesign());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator LevelDesign()
    {
        PlayerManager playerManager = PlayerManager.Instance;
        EnemyManager enemyManager = EnemyManager.Instance;
        
        Debug.Log("Level");
        while (!GridManager.Instance.hasInstantiated)
        {
            yield return null;
        }

        Debug.Log("Level hazırlanıyor");

        string[] lines = currentLevel.LevelLayout.text.Split('\n', System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            for (int j = 0; j < line.Length; j++)
            {
                if (line[j].Equals('0'))
                {
                    continue;
                }

                if (line[j].Equals('#'))
                {
                    Debug.Log("Player bulundu");
                    var player = Instantiate(Player, new Vector3(i, 1.4f, j), quaternion.identity);
                    var grid = GridManager.Instance.getGridFromLocation(new Vector3(i, 0, j));
                    player.GetComponent<Player>().SetGridStart(grid.gameObject, 1.4f);
                    playerManager.playerListForEnemyAI.Add(player);
                }

                if (line[j].Equals('$'))
                {
                    Debug.Log("EnemyBulundu");
                    var Enemy = EnemyList.GetRandom();
                    var EnemyObj = Instantiate(Enemy, new Vector3(i, 1.4f, j),
                        Quaternion.identity);
                    
                    var grid = GridManager.Instance.getGridFromLocation(new Vector3(i, 1.4f, j));
                    grid.GridObject = EnemyObj;
                    
                    if(EnemyObj.GetComponent<EnemyBrain>() != null)
                        EnemyObj.GetComponent<EnemyBrain>().SetGrid(grid);
                    
                    enemyManager.enemyListForEnemyAI.Add(EnemyObj);
                }
            }
        }
    }
}