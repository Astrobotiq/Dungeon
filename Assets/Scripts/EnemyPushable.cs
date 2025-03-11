using System;
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
    
    [SerializeField]
    private float crashStartTime;
    
    [SerializeField]
    private float crashRecoveryTime;
    
    private EnemyBrain _enemyBrain;

    void Start()
    {
        _enemyBrain = GetComponent<EnemyBrain>();
    }

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
                Push(targetGrid,currentGrid, diffrence);
            }
        }
        else
        {
            Crash(targetGrid,currentGrid); 
        }
    }

    private void Crash(Grid targetGrid, Grid currentGrid)
    {
        var StartPos = transform.position;

        var diffrence = targetGrid.transform.position - transform.position;
        diffrence = new Vector3(diffrence.x, 0, diffrence.z);
        

        transform.DOMove(transform.position + (diffrence / 2), crashStartTime).OnComplete((() =>
        {
            transform.DOMove(StartPos, crashRecoveryTime).OnComplete((() =>
            {
                FeelManager.Instance.ShakeCamera();
                targetGrid.GridObject.GetComponent<IHealth>().TakeDamage(5);
            }));
        }));
        /*
        transform.DOMove(targetGrid.transform.position, duration).OnComplete((() =>
        {
            transform.DOMove(currentGrid.transform.position,duration);
            if (targetGrid && targetGrid.GridObject && targetGrid.GridObject.GetComponent<IDamagable>())
            {
                targetGrid.GridObject.GetComponent<IDamagable>().Damage(1);
            }
        }));*/
    }

    private void Push(Grid targetGrid,Grid currentGrid, Vector3 diffrence)
    {
        if (TryGetComponent<LineController>( out var lineController))
        {
            lineController.RemoveLine();
        }

        var grid = _enemyBrain.GetTargetGrid();
        Grid newGrid = null;
        if (grid != null)
        {
            var gridPos = grid.transform.position;
            var newGridPos = gridPos + diffrence;
            newGrid = GridManager.Instance.getGridFromLocation(newGridPos);
            Debug.Log($"new Grid : {newGrid.transform.position}");
        }
        
        var pos = new Vector3(targetGrid.transform.position.x,transform.position.y, targetGrid.transform.position.z);
        transform.DOMove(pos, duration).SetEase(curve).OnComplete((() =>
        {
            currentGrid.GridObject = null;
            targetGrid.GridObject = gameObject;
            if (newGrid != null)
                _enemyBrain.SetTargetGrid(newGrid);
        }));
    }
}