using UnityEngine;
using UnityEngine.UI;

public class MountainHover : MonoBehaviour
{
    [SerializeField] 
    private GameObject MountainPopupHealthCanvas;

    public bool inAttackPreview = false;
    
    private void OnMouseEnter()
    {
        if(MountainPopupHealthCanvas==null)
        {
            Debug.Log("Mountain Health Canvas assign edilmemiştir");
            return;
        }
        
        if(inAttackPreview)
            return;
        
        MountainPopupHealthCanvas.SetActive(true);
        
        MountainPopupHealthCanvas.transform.LookAt(Camera.main.transform.position, Vector3.up);

        Slider slider = MountainPopupHealthCanvas.transform.GetChild(0).gameObject.GetComponent<Slider>();
        Debug.Log("Mountain Hover Health test health: " + GetComponent<MountainHealth>().getHealthPercentage());
        slider.value = GetComponent<MountainHealth>().getHealthPercentage();
    }

    private void OnMouseExit()
    {
        MountainPopupHealthCanvas.SetActive(false);
    }
}
