using UnityEngine;

public abstract class ITurn : MonoBehaviour
{
    [SerializeField]
    protected TurnNames nextTurn;

    protected ITurn GetNextTurn()
    {
        switch (nextTurn)
        {
            case TurnNames.EnemyTakePosition:
                return GetComponent<EnemyCombatPositionTurn>();
            case TurnNames.EnemySpawnLocation:
                return GetComponent<EnemySpawnLocation>();
            default:
                return null;
        }
    }
    
    public abstract void EnterTurn();

    public abstract void ExitTurn();
}

public enum TurnNames
{
    EnemyTakePosition = 1,
    EnemySpawnLocation = 2,
    PlayerTurn = 3,
    LevelEffects = 4,
    EnemyAttacks = 5,
    EnemySpawns = 6,
}