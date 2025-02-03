using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyRotateable : IRotatable
{
    public int RotateValue = -90;
    
    [SerializeField] 
    private float duration;

    [SerializeField] 
    private AnimationCurve curve;

    
    
    public override void Rotate(Vector3 direction, int rotateDegree)
    {
        Vector3 temp = transform.rotation.eulerAngles;
        transform.DORotate(new Vector3(temp.x, temp.y + RotateValue, temp.z), duration).SetEase(curve);
    }
}
