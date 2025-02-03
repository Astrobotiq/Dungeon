using System;
using System.Collections;
using DG.Tweening;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UIElements;

public class ThunderSkillScript : ISkillEffect
{
    [SerializeField] 
    private float duration;
    
    [SerializeField] 
    private int damageAmount;
    
    public override void StartMoving(Grid targetGrid) {
        Vector3 targetLoc = targetGrid.gameObject.transform.position;
        transform.DOMove(new Vector3(targetLoc.x, 1.4f, targetLoc.z), duration);
        StartCoroutine(Timer());
        
        IEnumerator Timer()
        {
            yield return new WaitForSeconds(duration+0.1f);
            ApplyEffect(targetGrid);
        }
    }

    public override void ApplyEffect(Grid targetGrid) {
        Debug.LogError("naber canım ben amcanım");
        Debug.LogError(" naber işte bunun destroy olması lazım " + gameObject);
        if (targetGrid.GridObject.GetComponent<IDamagable>() != null)
        {
            targetGrid.GridObject.GetComponent<IDamagable>().Damage(damageAmount);
        }
        Destroy(this.gameObject);
    }
}