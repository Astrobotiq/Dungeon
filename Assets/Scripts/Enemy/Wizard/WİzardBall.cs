using UnityEngine;

public class WİzardBall : ISkillEffect
{
    private Grid _targetGrid;
    
    [SerializeField]
    private int damageAmount = 3;
    public override void StartMoving(Grid targetGrid)
    {
        _targetGrid = targetGrid;
        ApplyEffect(targetGrid);
    }

    public override void ApplyEffect(Grid targetGrid)
    {
        if (_targetGrid.GridObject != null)
        {
            _targetGrid.GridObject.GetComponent<IHealth>().TakeDamage(damageAmount);
        }
        
        Destroy(gameObject);
    }
}
