using DG.Tweening;
using UnityEngine;

public class PlayerPushable : IPushable
{
    
    [SerializeField]
    private Player player;
    
    [SerializeField]
    private Move move;
    
    [SerializeField]
    private IDamagable damagable;

    void Awake()
    {
        player = GetComponent<Player>();

        move = GetComponent<Move>();
    }
    
    public override void Push(Vector3 position)
    {
        var diffrence = transform.position - position;
        var newPosition = transform.position + diffrence;

        var currentGrid = GridManager.Instance.getGridFromLocation(transform.position);
        var targetGrid = GridManager.Instance.getGridFromLocation(newPosition);

        if (targetGrid)
        {
            if (targetGrid.GridObject)
            {
                Crash(targetGrid,currentGrid);
            }
            else
            {
                move.StartMove(currentGrid,targetGrid);
                player.onPositionChange(targetGrid.gameObject);
            }
        }
        else
        {
            Crash(targetGrid,currentGrid); 
        }
        damagable.Damage(1);
    }

    private void Crash(Grid targetGrid, Grid currentGrid)
    {
        transform.DOMove(targetGrid.transform.position, duration).OnComplete((() =>
        {
            transform.DOMove(currentGrid.transform.position,duration);
            if (targetGrid && targetGrid.GridObject && targetGrid.GridObject.GetComponent<IDamagable>())
            {
                targetGrid.GridObject.GetComponent<IDamagable>().Damage(1);
            }
        }));
    }
}