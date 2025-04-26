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
    
    [SerializeField] 
    private SoundManager soundManager;
    
    public float LightningHitSoundVolume = 1f;
    
    public override void StartMoving(Grid targetGrid) {
        Vector3 targetLoc = targetGrid.gameObject.transform.position;
        transform.position = new Vector3(targetLoc.x, targetLoc.y + 0.6f, targetLoc.z);
        
        soundManager = GameObject.FindWithTag("SoundManager").GetComponent<SoundManager>();
        
        Timed.Run((() => ApplyEffect(targetGrid)), duration + 0.1f);
    }

    public override void ApplyEffect(Grid targetGrid) {

        if (targetGrid.GridObject != null)
        {
            if (targetGrid.GridObject.GetComponent<IDamagable>() != null)
            {
                targetGrid.GridObject.GetComponent<IDamagable>().Damage(damageAmount);
            }
            
            soundManager.PlaySound(SoundType.LightningHit,LightningHitSoundVolume);
            
            Destroy(this.gameObject);
        }
        
    }
}