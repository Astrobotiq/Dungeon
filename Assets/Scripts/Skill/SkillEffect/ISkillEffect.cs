using UnityEngine;

public abstract class ISkillEffect : MonoBehaviour
{
    public int DamageAmount;
    //After instantiating Effect Object we should call this function
    //This function will call directly apply effect in close combat and others will discussed
    public abstract void StartMoving(Grid targetGrid);

    //As name suggest effect should apply the target here.
    //This maybe called from onTrigger Enter or startMoving function
    public abstract void ApplyEffect(Grid targetGrid);
}
