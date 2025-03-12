using System.Collections;
using DG.Tweening;
using Unity.Collections;
using UnityEngine;

public class RotateSkillScript : ISkillEffect
{
    [SerializeField,Range(0.1f,5)] 
    float jumpPower = 1;
    
    [SerializeField, ReadOnly]
    int jumpNumber = 1;
    
    [SerializeField] 
    float jumpDuration = 1;
    
    public override void StartMoving(Grid targetGrid) {
        Vector3 targetLoc = targetGrid.gameObject.transform.position;
        transform.position = new Vector3(targetLoc.x, targetLoc.y + 0.6f, targetLoc.z);

        Timed.Run((() => ApplyEffect(targetGrid)), jumpDuration);
    }

    public override void ApplyEffect(Grid targetGrid) {

        if (targetGrid.gameObject && targetGrid.GridObject && targetGrid.GridObject.GetComponent<IRotatable>())
        {
            targetGrid.GridObject.GetComponent<IRotatable>().Rotate(Vector3.up, 90); //This will do not rotate because Rotate didn't overriden by
        }
        Destroy(gameObject);
    }
}
