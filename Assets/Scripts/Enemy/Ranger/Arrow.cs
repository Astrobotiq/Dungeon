using DG.Tweening;
using UnityEngine;

public class Arrow : ISkillEffect
{
    [SerializeField, Range(0f,5f)]
    private float speed = 3f;
    
    [SerializeField]
    private int damageAmount = 3;
    
    [SerializeField]
    private AnimationCurve ease;
    
    private Grid _targetGrid;
    
    public override void StartMoving(Grid targetGrid)
    {
        _targetGrid = targetGrid;
        float duration = Vector3.Distance(transform.position, _targetGrid.transform.position) / speed;
        
        if (_targetGrid.GridObject != null)
        {
            transform.DOMove(_targetGrid.GridObject.transform.position, duration).SetEase(ease).OnComplete(() => ApplyEffect(targetGrid));
        }
        else
        {
            Vector3 difference = _targetGrid.transform.position - transform.position;
            difference = new Vector3(difference.x, 0, difference.z);
            Vector3 currentPos = transform.position;

            Vector2Int direction;
            if (Mathf.Abs(difference.x) > Mathf.Abs(difference.z))
            {
                direction = difference.x > 0 ? Vector2Int.right : Vector2Int.left;
            }
            else
            {
                direction = difference.z > 0 ? Vector2Int.up : Vector2Int.down;
            }

            for (int i = 0; i < 10; i++)
            {
                Vector3 nextPos = new Vector3(currentPos.x + direction.x,currentPos.y,currentPos.z + direction.y);
                
                Grid grid = GridManager.Instance.getGridFromLocation(nextPos);
                if (grid == null)
                {
                    Debug.Log("Arama tahtanın dışına çıktı." +
                              $"Next pos : {nextPos}");
                    break;
                }

                if (grid.GridObject != null)
                {
                    Debug.Log("Yeni Grid bulundu." +
                              $"name : {grid.GridObject.gameObject.name}" +
                              $"pos  : {grid.transform.position}");
                    _targetGrid = grid;
                    transform.DOMove(_targetGrid.GridObject.transform.position, duration).SetEase(ease).OnComplete(() => ApplyEffect(_targetGrid));
                    return;
                }

                currentPos = nextPos;
            }
            
            var newPos = transform.position + difference * 5;
            transform.DOMove(newPos, duration).SetEase(ease).OnComplete(() => Destroy(gameObject));
        }
    }

    public override void ApplyEffect(Grid targetGrid)
    {
        if (_targetGrid.GridObject != null)
        {
            _targetGrid.GridObject.GetComponent<IHealth>().TakeDamage(damageAmount);
            FeelManager.Instance.ShakeCamera();
        }
        
        Destroy(gameObject);
    }
}