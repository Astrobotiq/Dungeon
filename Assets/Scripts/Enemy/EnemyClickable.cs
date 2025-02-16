using UnityEngine;
using UnityEngine.UIElements;

public class EnemyClickable : IClickable
{
    [SerializeField]
    private EnemyBrain enemy;

    public override void onLeftClick()
    {
        Debug.Log("Enemy seçildi");
        enemy.OnEnemySelection(true);
    }

    public override void onRightClick()
    {
        throw new System.NotImplementedException();
    }
}
