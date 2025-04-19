using System.Collections;
using DG.Tweening;
using UnityEngine;

public class ArmController : Singleton<ArmController>
{
    [SerializeField]
    private float yOffset;
    
    [SerializeField]
    private GameObject Arm;

    public Vector3 GetPosition => transform.position;
    

    public void StartInstantiate(GameObject enemy, Vector3 pos, float duration, int damage)
    {
        if (!Arm.gameObject.activeInHierarchy)
        {
            SetArmVisible(true);
        }
        
        var startPos = transform.position;
        enemy.transform.SetParent(transform);
        var grid = GridManager.Instance.getGridFromLocation(pos);

        if (grid.GridObject != null)
        {
            InstantiateFail();
        }
        else
        {
            InstantiateSuccess();
        }
        
        void InstantiateSuccess()
        {
            transform.DOMove(pos, duration).OnComplete((() =>
            {
                enemy.transform.SetParent(null);
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

    public void SetArmVisible(bool visible)
    {
        Arm.gameObject.SetActive(visible);
    }
}
