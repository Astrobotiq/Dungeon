using MoreMountains.Feedbacks;
using UnityEngine;

public class FeelManager : Singleton<FeelManager>
{
    [SerializeField]
    private MMF_Player cameraShaker;
    
    [SerializeField]
    private MMF_Player playerShaker;
    
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
}
