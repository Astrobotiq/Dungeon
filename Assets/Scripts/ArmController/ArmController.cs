using System.Collections;
using DG.Tweening;
using UnityEngine;

public class ArmController : Singleton<ArmController>
{
    [SerializeField]
    private float yOffset;

    public Vector3 GetPosition => transform.position;
    

    public void StartInstantiate(GameObject enemy, Vector3 pos, float duration, int damage)
    {
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
                transform.DOMove(startPos, duration);
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
                transform.DOMove(startPos, duration).OnComplete((() =>
                {
                    enemy.transform.SetParent(null);
                    Destroy(enemy);
                }));
            }));
        }
        
    }

    public void RemoveEnemyFromTable(Vector3 pos, float duration)
    {
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
            }));
        }));
    }
}
