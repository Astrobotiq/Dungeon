using System.Collections;
using DG.Tweening;
using Unity.Collections;
using UnityEngine;

public class Fireball : ISkillEffect
{
    [SerializeField,Range(0.1f,5)] float jumpPower = 1;
    [SerializeField, ReadOnly] int jumpNumber = 1;
    [SerializeField] float jumpDuration = 1;
    public int DamageAmount;
    [SerializeField] Grid _target;

    public override void StartMoving(Grid targetGrid)
    {
        _target = targetGrid;
        transform.DOJump(targetGrid.gameObject.transform.position, jumpPower, jumpNumber, jumpDuration);
        StartCoroutine(Timer());

        IEnumerator Timer()
        {
            yield return new WaitForSeconds(jumpDuration);
            ApplyEffect();
        }
    }

    public override void ApplyEffect(Grid targetGrid = null)
    {
        if (_target.GridObject && _target.GridObject.GetComponent<IDamagable>() != null)
        {
            _target.GridObject.GetComponent<IDamagable>().Damage(DamageAmount);
        }

        var pos = _target.gameObject.transform.position;

        for (int i = -1; i < 1; i++)
        {
            if (i == 0)
            {
                continue;
            }

            var xGrid = GridManager.Instance.getGridFromLocation(new Vector3(pos.x + i, pos.y, pos.z));
            var zGrid = GridManager.Instance.getGridFromLocation(new Vector3(pos.x, pos.y, pos.z + i));

            if (xGrid.gameObject && xGrid.GridObject && xGrid.GridObject.GetComponent<IPushable>())
            {
                xGrid.GridObject.GetComponent<IPushable>().Push(pos);
            }
            
            if (zGrid.gameObject && zGrid.GridObject && zGrid.GridObject.GetComponent<IPushable>())
            {
                zGrid.GridObject.GetComponent<IPushable>().Push(pos);
            }
        }
        
        Destroy(this.gameObject);
    }
}
