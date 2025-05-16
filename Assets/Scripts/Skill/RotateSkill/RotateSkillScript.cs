using System.Collections;
using DG.Tweening;
using Unity.Collections;
using UnityEngine;

public class RotateSkillScript : ISkillEffect
{
    
    [SerializeField] 
    float rotateDuration = 1;
    
    [SerializeField] 
    private SoundManager soundManager;

    public float RotateHitSoundVolume = 1f;
    
    public override void StartMoving(Grid targetGrid) {
        Vector3 targetLoc = targetGrid.gameObject.transform.position;
        transform.position = new Vector3(targetLoc.x, targetLoc.y + 0.6f, targetLoc.z);
        
        soundManager = GameObject.FindWithTag("SoundManager").GetComponent<SoundManager>();

        Timed.Run((() => ApplyEffect(targetGrid)), rotateDuration/2);
    }

    public override void ApplyEffect(Grid targetGrid) {

        if (targetGrid.gameObject && targetGrid.GridObject && targetGrid.GridObject.GetComponent<IRotatable>())
        {
            targetGrid.GridObject.GetComponent<IRotatable>().Rotate(Vector3.up, 90); //This will do not rotate because Rotate didn't overriden by
            
            //soundManager.PlaySound(SoundType.RotateHit,RotateHitSoundVolume);
        }
        Timed.Run((() => Destroy(gameObject)),rotateDuration/2);
    }
}
