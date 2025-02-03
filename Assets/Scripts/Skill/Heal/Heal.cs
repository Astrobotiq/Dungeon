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

    void OnValidate()
    {
        effect = GetComponent<MMF_Player>();
    }

    public override void StartMoving(Grid targetGrid)
    {
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
                effect.PlayFeedbacks();
            }
        }
        
        //StartCoroutine(Timer());

        IEnumerator Timer()
        {
            yield return new WaitForSeconds(healTime);
            Destroy(this.gameObject);
        }
    }
}
