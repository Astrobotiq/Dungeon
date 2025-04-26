using System;
using System.Collections;
using DG.Tweening;
using Unity.Collections;
using UnityEngine;

public class Fireball : ISkillEffect
{
    [SerializeField,Range(0.1f,5)] 
    float jumpPower = 1;
    
    [SerializeField, ReadOnly] 
    int jumpNumber = 1;
    
    [SerializeField] 
    float jumpDuration = 1;
    
    [SerializeField]
    private int damage;
    
    [SerializeField] 
    Grid _target;
    
    [SerializeField]
    AnimationCurve curve;
    
    public int DamageAmount;

    [SerializeField] 
    private SoundManager soundManager;

    public float FireballSentSoundVolume = 1f;
    public float FireballHitSoundVolume = 1f;
    
    public override void StartMoving(Grid targetGrid)
    {
        _target = targetGrid;
        transform.DOJump(targetGrid.gameObject.transform.position, jumpPower, jumpNumber, jumpDuration).SetEase(curve);
        
        soundManager = GameObject.FindWithTag("SoundManager").GetComponent<SoundManager>();
        
        Timed.Run((() =>
        {
            FeelManager.Instance.ShakeCamera();
            soundManager.PlaySound(SoundType.FireballSent,FireballSentSoundVolume);
            ApplyEffect();
        }),jumpDuration);
    }

    public override void ApplyEffect(Grid targetGrid = null)
    {
        soundManager.PlaySound(SoundType.FireballHit,FireballHitSoundVolume);
        
        if (_target.GridObject && _target.GridObject.GetComponent<IDamagable>() != null)
        {
            _target.GridObject.GetComponent<IDamagable>().Damage(DamageAmount);
        }

        var pos = _target.gameObject.transform.position;
        
        for (int i = -1; i <= 1; i++)
        {
            if (i == 0)
            {
                continue;
            }

            var xGrid = GridManager.Instance.getGridFromLocation(new Vector3(pos.x + i, pos.y, pos.z));
            var zGrid = GridManager.Instance.getGridFromLocation(new Vector3(pos.x, pos.y, pos.z + i));

            if (xGrid)
            {
                Debug.Log("x:" + xGrid.transform.position);
            }
            
            if (zGrid)
            {
                Debug.Log("z:" + zGrid.transform.position);
            }

            if (xGrid && xGrid.gameObject && xGrid.GridObject)
            {
                Debug.Log("XObject");
                if (xGrid.GridObject.GetComponent<IPushable>())
                {
                    Debug.Log("XPushable");
                    xGrid.GridObject.GetComponent<IPushable>().Push(pos);
                }
            }
            
            if (zGrid && zGrid.gameObject && zGrid.GridObject)
            {
                Debug.Log("ZObject");
                if (zGrid.GridObject.GetComponent<IPushable>())
                {
                    Debug.Log("ZPushable");
                    zGrid.GridObject.GetComponent<IPushable>().Push(pos);
                }
            }
        }

        Timed.Run((() => Destroy(gameObject)), 1.2f);
    }
}
