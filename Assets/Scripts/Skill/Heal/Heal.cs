using System;
using System.Collections;
using MoreMountains.Feedbacks;
using UnityEngine;

public class Heal : ISkillEffect
{
    [SerializeField]
    private int heal;
    
    [SerializeField]
    private float healTime = 4;
    
    [SerializeField]
    private MMF_Player effect;
    
    [SerializeField] 
    private SoundManager soundManager;
    
    public float HealHitSoundVolume = 1f;

    void OnValidate()
    {
        effect = GetComponent<MMF_Player>();
    }

    public override void StartMoving(Grid targetGrid)
    {
        soundManager = GameObject.FindWithTag("SoundManager").GetComponent<SoundManager>();
        
        ApplyEffect(targetGrid);
    }

    public override void ApplyEffect(Grid targetGrid)
    {
        transform.position = targetGrid.transform.position;

        if (targetGrid.gameObject && targetGrid.GridObject)
        {
            if (targetGrid.GridObject.GetComponent<IHealth>())
            {
                targetGrid.GridObject.GetComponent<IHealth>().Heal(heal);
                
                soundManager.PlaySound(SoundType.HealHitSound,HealHitSoundVolume);
                InGameUITextMesh.Instance.UpdatePlayerBars();
                
                effect.PlayFeedbacks();
            }
        }

        Timed.Run((() => Destroy(gameObject)), healTime);
    }
}
