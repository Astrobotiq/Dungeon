using MoreMountains.Feedbacks;
using UnityEngine;

public class FeelManager : Singleton<FeelManager>
{
    [SerializeField]
    private MMF_Player cameraShaker;
    
    [SerializeField]
    private MMF_Player playerShaker;

    [SerializeField] 
    private MMF_Player CameraLooker;
    
    public void ShakeCamera()
    {
        cameraShaker.PlayFeedbacks();
    }

    public void ShakeGameObject(MMPositionShaker shaker)
    {
        MMF_PositionShake positionShaker = null;

        foreach (MMF_Feedback feedback in playerShaker.FeedbacksList)
        {
            if (feedback is MMF_PositionShake)
            {
                positionShaker = (MMF_PositionShake)feedback;
                break;
            }
        }

        positionShaker.TargetShaker = shaker;
        playerShaker.PlayFeedbacks();

    }

    public void CameraLookAt(Transform cameraTransform, Transform targetTransform, float duration)
    {
        MMF_LookAt looker = null; 
        foreach (MMF_Feedback feedback in CameraLooker.FeedbacksList)
        {
            if (feedback is MMF_LookAt)
            {
                looker = (MMF_LookAt)feedback;
                break;
            }
        }

        looker.TransformToRotate = cameraTransform;
        looker.LookAtTarget = targetTransform;
        looker.Duration = duration;
        
        CameraLooker.PlayFeedbacks();
    }
    
    
}
