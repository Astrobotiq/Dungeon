using DG.Tweening;
using Unity.Collections;
using UnityEngine;

public class Fireball : ISkillEffect
{
    [SerializeField,Range(0.1f,5)] float jumpPower = 1;
    [SerializeField, ReadOnly] int jumpNumber = 1;
    [SerializeField] float jumpDuration = 1;
    public int DamageAmount;

    public override void StartMoving(Grid targetGrid)
    {
        transform.DOJump(targetGrid.gameObject.transform.position, jumpPower, jumpNumber, jumpDuration).
            OnComplete((() =>
            {
                ApplyEffect(targetGrid);
                //At some point we should create an smash particle something
            }));
    }

    public override void ApplyEffect(Grid targetGrid)
    {
        if (targetGrid.GridObject.GetComponent<IDamagable>() != null)
        {
            targetGrid.GridObject.GetComponent<IDamagable>().Damage(DamageAmount);
        }

        var pos = targetGrid.gameObject.transform.position;

        for (int i = -1; i < 1; i++)
        {
            if (i == 0)
            {
                continue;
            }

            var xGrid = GridManager.Instance.getGridFromLocation(new Vector3(pos.x + i, pos.y, pos.z));
            var zGrid = GridManager.Instance.getGridFromLocation(new Vector3(pos.x, pos.y, pos.z + i));

            if (xGrid.gameObject != null && xGrid.GridObject != null && xGrid.GridObject.GetComponent<IPushable>() != null)
            {
                xGrid.GridObject.GetComponent<IPushable>().Push(pos);
            }
            
            if (zGrid.gameObject != null && zGrid.GridObject != null && zGrid.GridObject.GetComponent<IPushable>() != null)
            {
                zGrid.GridObject.GetComponent<IPushable>().Push(pos);
            }
        }
    }
}
