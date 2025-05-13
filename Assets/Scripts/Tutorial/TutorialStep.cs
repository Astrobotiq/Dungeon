using UnityEngine;
using UnityEngine.UI;

public class TutorialStep : MonoBehaviour
{
    public TutorialType TutorialType;
    public GameObject TutorialPanel;
    public Button OkBtn;

    private bool hasBeenShown = false;

    public void EnterTutorial()
    {
        if (hasBeenShown) return;
        
        hasBeenShown = true;
        TutorialPanel.SetActive(true);
        OkBtn.onClick.AddListener((() => ExitTutorial()));
    }

    public void ExitTutorial()
    {
        OkBtn.onClick.RemoveAllListeners();
        TutorialPanel.SetActive(false);
        TutorialManager.Instance.CloseTutorial();
    }
}