using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialLevel", menuName = "TutorialLevel")]
public class TutorialLevelSO : ScriptableObject
{
    [SerializeField]
    private List<TextAsset> tutorialSteps;

    public List<TextAsset> GetTutorialSteps => tutorialSteps;
}