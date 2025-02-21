using UnityEngine;

public abstract class AbstractEnemyAI_Calculator : MonoBehaviour
{
    public enum AttackedObjectType {
        PlayerType,
        EnemyType,
        StatueType
    }

    public abstract void CalculateGridAttackValues(GameObject gameObject, AttackedObjectType type);

    public abstract void CalculateGridMoveValues(GameObject gameObject, AttackedObjectType type, int enemyWalkDistance);
}
