using DG.Tweening;
using UnityEngine;

public class EnemyPushable : IPushable
{
    [SerializeField]
    private float duration;
    
    [SerializeField]
    private Move move;
    
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
            Debug.Log("gidecek yerim var");
            if (targetGrid.GridObject)
            {
                Debug.Log("Mersin");
                Crash(targetGrid,currentGrid);
            }
            else
            {
                Debug.Log("yalova");
                move.StartMove(currentGrid,targetGrid);
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