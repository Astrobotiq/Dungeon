using DG.Tweening;
using UnityEngine;

public class EnemyRotateable : IRotatable
{
    public int RotateValue = -90;
    
    [SerializeField] 
    private float duration;

    [SerializeField] 
    private AnimationCurve curve;
    
    private EnemyBrain _enemyBrain;

    void Start()
    {
        _enemyBrain = GetComponent<EnemyBrain>();
    }

    
    
    public override void Rotate(Vector3 direction, int rotateDegree)
    {
        if (TryGetComponent<LineController>(out var lineController))
        {
            lineController.RemoveLine();
        }
        
        var grid = _enemyBrain.GetTargetGrid();
        Grid newGrid = null;
        if (grid != null)
        {
            var diffrence = new Vector3(grid.transform.position.x - transform.position.x, 0,
                grid.transform.position.z - transform.position.z);
            
            if (diffrence.z>0 || diffrence.z<0)
            {
                diffrence = new Vector3(diffrence.x, diffrence.y, -diffrence.z);
            }
            
            var newGridPos = new Vector3(transform.position.x + diffrence.z, transform.position.y, transform.position.z + diffrence.x);
            
            if (newGridPos.x>=GridManager.Instance.EndPosition)
            {
                newGridPos = new Vector3(7, transform.position.y, transform.position.z - diffrence.x);
            }else if (newGridPos.x<0)
            {
                newGridPos = new Vector3(0, transform.position.y, transform.position.z - diffrence.x);
            }
            
            if (newGridPos.z>=GridManager.Instance.EndPosition)
            {
                newGridPos = new Vector3(transform.position.x - diffrence.z, transform.position.y, 7);
            }else if (newGridPos.x<0)
            {
                newGridPos = new Vector3(transform.position.x - diffrence.z, transform.position.y, 0);
            }

            //Check if pos out of bounds
            if (newGridPos.x>7)
            {
                newGridPos = new Vector3(7, newGridPos.y, newGridPos.z);
            }
            else if (newGridPos.x<0)
            {
                newGridPos = new Vector3(0, newGridPos.y, newGridPos.z);
            }
            
            if (newGridPos.z>7)
            {
                newGridPos = new Vector3(newGridPos.x, newGridPos.y, 7);
            }
            else if (newGridPos.z<0)
            {
                newGridPos = new Vector3(newGridPos.x, newGridPos.y, 0);
            }
            
            newGrid = GridManager.Instance.getGridFromLocation(newGridPos);
            Debug.Log($"new Grid : {newGrid.transform.position}");
        }
        
        Vector3 temp = transform.rotation.eulerAngles;
        transform.DORotate(new Vector3(temp.x, temp.y + RotateValue, temp.z), duration).SetEase(curve).OnComplete(
            (() =>
            {
                if (newGrid != null)
                {
                    _enemyBrain.SetTargetGrid(newGrid);
                    lineController.DrawLine(transform.position,new Vector3(newGrid.transform.position.x,transform.position.y,newGrid.transform.position.z));
                }
            }));
    }
}
