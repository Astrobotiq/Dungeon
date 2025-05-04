using UnityEngine;
using UnityEngine.UI;

public class DrumHover : MonoBehaviour
{
    [SerializeField] 
    private GameObject drumPopupHealthCanvas;
    
    public bool inAttackPreview = false;
    
    private void OnMouseEnter()
    {
        if(drumPopupHealthCanvas==null)
        {
            Debug.Log("Village Health Canvas assign edilmemiştir");
            return;
        }
        
        if(inAttackPreview)
            return;
        
        drumPopupHealthCanvas.SetActive(true);
        
        drumPopupHealthCanvas.transform.LookAt(Camera.main.transform.position, Vector3.up);

        Slider slider = drumPopupHealthCanvas.transform.GetChild(0).gameObject.GetComponent<Slider>();
        //Debug.Log("Village Hover Health test health: " + GetComponent<DrumHealth>().getHealthPercentage());
        slider.value = GetComponent<DrumHealth>().getHealthPercentage();
    }

    private void OnMouseExit()
    {
        drumPopupHealthCanvas.SetActive(false);
    }
}
