using DG.Tweening;
using UnityEngine;

public class PlayerPushable : IPushable
{
    [SerializeField]
    private float duration;
    
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
        var absDiffrence = new Vector3(Mathf.Abs(diffrence.x),Mathf.Abs(diffrence.y),Mathf.Abs(diffrence.z));
        var newPosition = transform.position + absDiffrence;

        var currentGrid = GridManager.Instance.getGridFromLocation(transform.position);
        var targetGrid = GridManager.Instance.getGridFromLocation(newPosition);

        if (targetGrid)
        {
            move.StartMove(currentGrid,targetGrid);
            player.onPositionChange(targetGrid.gameObject);
            return;
        }

        transform.DOMove(targetGrid.transform.position, duration).OnComplete((() =>
        {
            transform.DOMove(currentGrid.transform.position,duration);
        }));
        
        damagable.Damage(1);
    }
}