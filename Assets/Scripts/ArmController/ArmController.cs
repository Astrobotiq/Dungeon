using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ArmController : Singleton<ArmController>
{
    [SerializeField]
    private float yOffset;
    
    [SerializeField]
    private GameObject Arm;
    public Vector3 GetPosition => transform.position;

    public Vector3 StartPosition;

    void Start()
    {
        StartPosition = transform.position;
    }

    private Queue<IEnumerator> armTaskQueue = new Queue<IEnumerator>();
    
    private bool isProcessing = false;
    
    public void EnqueueStartInstantiate(GameObject enemy, Vector3 pos, float duration, int damage)
    {
        armTaskQueue.Enqueue(StartInstantiateRoutine(enemy, pos, duration, damage));
        ProcessQueue();
    }

    public void EnqueueRemoveEnemy(Vector3 pos, float duration)
    {
        armTaskQueue.Enqueue(RemoveEnemyRoutine(pos, duration));
        ProcessQueue();
    }

    private void ProcessQueue()
    {
        if (isProcessing || armTaskQueue.Count == 0) return;

        StartCoroutine(ProcessTask());
    }

    private IEnumerator ProcessTask()
    {
        isProcessing = true;

        yield return StartCoroutine(armTaskQueue.Dequeue());
        yield return new WaitForSeconds(0.2f);

        isProcessing = false;

        // İş bitti, sıradaki varsa devam et
        ProcessQueue();
    }

    private IEnumerator StartInstantiateRoutine(GameObject enemy, Vector3 pos, float duration, int damage)
    {
        SetArmVisible(true);
        var startPos = transform.position;

        var grid = GridManager.Instance.getGridFromLocation(pos);

        if (grid.GridObject != null)
        {
            // InstantiateFail
            var newPos = new Vector3(pos.x, 1f, pos.z);
            enemy.transform.SetParent(transform);

            yield return transform.DOMove(newPos, duration).WaitForCompletion();

            var gridObject = grid.GridObject;
            gridObject.GetComponent<IHealth>().TakeDamage(damage);
            FeelManager.Instance.ShakeCamera();
            EventManager.Instance.InvokeOnSpawnerPrevented();

            yield return transform.DOMove(StartPosition, duration).WaitForCompletion();
            transform.position = StartPosition;

            enemy.transform.SetParent(null);
            Destroy(enemy);
        }
        else
        {
            // InstantiateSuccess
            enemy.transform.SetParent(transform);

            yield return transform.DOMove(pos, duration).WaitForCompletion();

            enemy.transform.SetParent(null);
            grid.GridObject = enemy;

            if (enemy.TryGetComponent<EnemyBrain>(out var enemyBrain))
                enemyBrain.SetGrid(grid);

            EnemyManager.Instance.enemyListForEnemyAI.Add(enemy);

            yield return transform.DOMove(StartPosition, duration).WaitForCompletion();
            transform.position = StartPosition;
        }

        SetArmVisible(false);
    }

    private IEnumerator RemoveEnemyRoutine(Vector3 pos, float duration)
    {
        SetArmVisible(true);
        var startPos = transform.position;
        var grid = GridManager.Instance.getGridFromLocation(pos);
        var enemy = grid.GridObject;
        var enemyPos = enemy.transform.position;

        yield return transform.DOMove(enemyPos, duration).WaitForCompletion();
        

        if (enemy == null)
            Debug.LogError("Bulduğun grid'in enemy'si yok");

        grid.GridObject = null;
        enemy.transform.SetParent(transform);

        yield return transform.DOMove(StartPosition, duration).WaitForCompletion();
        transform.position = StartPosition;
        
        Destroy(enemy);
        SetArmVisible(false);
    }

    public void SetArmVisible(bool visible)
    {
        Arm.gameObject.SetActive(visible);
    }
    
    //Old code
    public void StartInstantiate(GameObject enemy, Vector3 pos, float duration, int damage)
    {
        if (!Arm.gameObject.activeInHierarchy)
        {
            SetArmVisible(true);
        }
        
        var startPos = transform.position;
        enemy.transform.SetParent(transform);
        var grid = GridManager.Instance.getGridFromLocation(pos);
        
        Debug.Log($"Transform of grid : {grid.transform.position}");

        if (grid.GridObject != null)
        {
            InstantiateFail();
        }
        else
        {
            InstantiateSuccess(pos);
        }
        
        void InstantiateSuccess(Vector3 pos)
        {
            transform.DOMove(pos, duration).OnComplete((() =>
            {
                enemy.transform.SetParent(null);
                Debug.Log($"New transform of grid : {grid.gameObject.transform.position}");
                grid.GridObject = enemy;
                
                if(enemy.TryGetComponent<EnemyBrain>(out var enemyBrain))
                    enemyBrain.SetGrid(grid);
                    
                EnemyManager.Instance.enemyListForEnemyAI.Add(enemy);
                
                transform.DOMove(startPos, duration).OnComplete((() => SetArmVisible(false)));
            }));
        }
        
        void InstantiateFail()
        {
            var newPos = new Vector3(pos.x,1f,pos.z);
            
            transform.DOMove(newPos, duration).OnComplete((() =>
            {
                var gridObject = grid.GridObject;
                gridObject.GetComponent<IHealth>().TakeDamage(damage);
                FeelManager.Instance.ShakeCamera();
                
                EventManager.Instance.InvokeOnSpawnerPrevented();
                
                transform.DOMove(startPos, duration).OnComplete((() =>
                {
                    enemy.transform.SetParent(null);
                    Destroy(enemy);
                    SetArmVisible(false);
                }));
            }));
        }
        
    }

    public void RemoveEnemyFromTable(Vector3 pos, float duration)
    {
        if (!Arm.gameObject.activeInHierarchy)
        {
            SetArmVisible(true);
        }
        
        var startPos = transform.position;
        transform.DOMove(pos, duration).OnComplete((() =>
        {
            var grid = GridManager.Instance.getGridFromLocation(pos);
            var enemy = grid.GridObject;
            if (enemy == null)
            {
                Debug.LogError("Bulduğun grid'in enemy'si yok");
            }

            grid.GridObject = null;
            enemy.transform.SetParent(transform);
            transform.DOMove(startPos, duration).OnComplete((() =>
            {
                Destroy(enemy.gameObject);
                SetArmVisible(false);
            }));
        }));
    }

    
}
