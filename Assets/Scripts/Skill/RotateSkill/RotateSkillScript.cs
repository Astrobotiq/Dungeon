using System.Collections;
using DG.Tweening;
using Unity.Collections;
using UnityEngine;

public class RotateSkillScript : ISkillEffect
{
    [SerializeField,Range(0.1f,5)] float jumpPower = 1;
    [SerializeField, ReadOnly] int jumpNumber = 1;
    [SerializeField] float jumpDuration = 1;
    public int DamageAmount;
    [SerializeField] Grid _target;
    
    public override void StartMoving(Grid targetGrid) {
        _target = targetGrid;
        transform.DOJump(targetGrid.gameObject.transform.position, jumpPower, jumpNumber, jumpDuration);
        StartCoroutine(Timer());

        IEnumerator Timer()
        {
            yield return new WaitForSeconds(jumpDuration);
            ApplyEffect();
            Destroy(gameObject);
        }
    }

    public override void ApplyEffect(Grid targetGrid = null) {

        if (targetGrid.gameObject && targetGrid.GridObject && targetGrid.GridObject.GetComponent<IRotatable>())
        {
            targetGrid.GridObject.GetComponent<IRotatable>().Rotate(Vector3.up, 90); //This will do not rotate because Rotate didn't overriden by
        }
        
    }
}
