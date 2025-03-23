using UnityEngine;

public class EnemyFactory : Singleton<EnemyFactory>
{
    [SerializeField]
    private EnemyDB enemyDB;
    
    public GameObject Build(string name, Vector3 position, Quaternion quaternion)
    {
        EnemyType enemy = EnemyType.None;
        switch (name)
        {
            case FactoryParameters.Spider:
                enemy = EnemyType.Spider;
                break;
            case FactoryParameters.OrcWizard:
                enemy = EnemyType.Wizard;
                break;
            case FactoryParameters.OrcRanger:
                enemy = EnemyType.Ranger;
                break;
        }

        if (enemy == EnemyType.None)
        {
            Debug.LogWarning("Player type düzgün verilmemiş");
            return null;
        }

        var enemyGO = enemyDB.GetEnemyByType(enemy);
        
        var enemyobj = enemyGO.Item1;
        var enemyOffset = enemyGO.Item2;
        var pos = new Vector3(position.x,enemyOffset,position.z);
        return Instantiate(enemyobj, pos, quaternion);
    }

    public GameObject BuildRandom(Vector3 position, Quaternion quaternion)
    {
        var EnemySO = enemyDB.GetRandomEnemy();

        var enemy = EnemySO.Item1;
        var enemyOffset = EnemySO.Item2;
        
        if (enemy == null)
        {
            Debug.LogWarning("There is no Enemy In DB");
            return null;
        }

        var pos = new Vector3(position.x,enemyOffset,position.z);

        return Instantiate(enemy, pos, quaternion);
    }
}