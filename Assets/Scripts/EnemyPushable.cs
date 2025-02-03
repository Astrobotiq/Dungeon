using DG.Tweening;
using UnityEngine;

public class EnemyPushable : IPushable
{
    [SerializeField]
    private float duration;
    
    [SerializeField]
    private AnimationCurve curve;
    
    [SerializeField]
    private IDamagable damagable;
    public override void Push(Vector3 position)
    {
        var diffrence = transform.position - position;
        var newPosition = transform.position + diffrence;
        
        Debug.Log("diffrence : "+diffrence + "\n" +
                  "newPos  : " + newPosition + "\n" +
                  "transform pos :" + transform.position);

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
                Push(targetGrid);
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

    private void Push(Grid targetGrid)
    {
        var pos = new Vector3(targetGrid.transform.position.x,transform.position.y, targetGrid.transform.position.z);
        transform.DOMove(pos, duration).SetEase(curve);
    }
}