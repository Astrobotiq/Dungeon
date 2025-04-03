using UnityEngine;

public abstract class ITurn : MonoBehaviour
{
    [SerializeField]
    protected TurnNames nextTurn;

    public bool isLastTurn;

    protected ITurn GetNextTurn()
    {
        switch (nextTurn)
        {
            case TurnNames.EnemyTakePosition:
                return GetComponent<EnemyCombatPositionTurn>();
            case TurnNames.EnemySpawnLocation:
                return GetComponent<EnemySpawnLocation>();
            case TurnNames.PlayerTurn:
                return GetComponent<PlayerTurn>();
            case TurnNames.EnemyAttacks:
                return GetComponent<EnemyAttack>();
            case TurnNames.EnemySpawns:
                return GetComponent<EnemySpawnTurn>();
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