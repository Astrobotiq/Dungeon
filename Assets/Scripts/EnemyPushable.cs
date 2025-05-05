using System;
using DG.Tweening;
using UnityEngine;

public class EnemyPushable : IPushable
{
    
    [SerializeField]
    private AnimationCurve curve;
    
    [SerializeField]
    private IDamagable damagable;
    
    [SerializeField]
    private float crashStartTime;
    
    [SerializeField]
    private float crashRecoveryTime;
    
    private EnemyBrain _enemyBrain;
    
    [SerializeField] 
    private SoundManager soundManager;

    public float BumpSoundVolume = 1f;
    
    private Vector3 newPos;

    void Start()
    {
        _enemyBrain = GetComponent<EnemyBrain>();
        soundManager = GameObject.FindWithTag("SoundManager").GetComponent<SoundManager>();
    }

    public override void Push(Vector3 position)
    {
        var diffrence = transform.position - position;
        var newPosition = transform.position + diffrence;
        newPos = newPosition;
        
        Debug.Log("diffrence : "+diffrence + "\n" +
                  "newPos  : " + newPosition + "\n" +
                  "transform pos :" + transform.position);

        var currentGrid = GridManager.Instance.getGridFromLocation(transform.position);
        var targetGrid = GridManager.Instance.getGridFromLocation(newPosition);

        if (targetGrid)
        {
            if (targetGrid.GridObject && targetGrid.GridObject.GetComponent<Water>() == null)
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

        var diffrence = Vector3.zero;

        if (targetGrid)
        {
            diffrence = targetGrid.transform.position - transform.position;
        }
        else
        {
            diffrence = newPos - transform.position;
        }
        
        diffrence = new Vector3(diffrence.x, 0, diffrence.z);
        

        transform.DOMove(transform.position + (diffrence / 2), crashStartTime).OnComplete((() =>
        {
            transform.DOMove(StartPos, crashRecoveryTime).OnComplete((() =>
            {
                FeelManager.Instance.ShakeCamera();
                currentGrid.GridObject.GetComponent<IHealth>().TakeDamage(1);
                targetGrid.GridObject.GetComponent<IHealth>().TakeDamage(1);
                
                soundManager.PlaySound(SoundType.BumpSound,BumpSoundVolume);
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
            if (!newGrid)
            {
                newGrid = grid;
            }
            Debug.Log($"new Grid : {newGrid.transform.position}");
        }
        
        var pos = new Vector3(targetGrid.transform.position.x,transform.position.y, targetGrid.transform.position.z);
        transform.DOMove(pos, duration).SetEase(curve).OnComplete((() =>
        {
            if (TryGetComponent<SpiderEnemyBrain>(out var spider))
            {
                spider.DestroyWeb();
            }

            //If the target grid is water then we do not need to do this
            if (!targetGrid.GridObject || (targetGrid.GridObject && targetGrid.GridObject.GetComponent<Water>() == null))
            {
                currentGrid.GridObject = null;
                targetGrid.GridObject = gameObject;
                
                _enemyBrain.SetGrid(targetGrid);
                if (newGrid != null)
                    _enemyBrain.SetTargetGrid(newGrid);
            }
            
            EventManager.Instance.InvokeOnPush();
        }));
    }
}