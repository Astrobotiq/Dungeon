using UnityEngine;
using UnityEngine.UI;

public class VillageHover : MonoBehaviour
{
    [SerializeField] 
    private GameObject villagePopupHealthCanvas;
    
    private void OnMouseEnter()
    {
        if(villagePopupHealthCanvas==null)
        {
            Debug.Log("Village Health Canvas assign edilmemiştir");
            return;
        }
        villagePopupHealthCanvas.SetActive(true);
        
        villagePopupHealthCanvas.transform.LookAt(Camera.main.transform.position, Vector3.up);

        Slider slider = villagePopupHealthCanvas.transform.GetChild(0).gameObject.GetComponent<Slider>();
        Debug.Log("Village Hover Health test health: " + GetComponent<VillageHealth>().getHealthPercentage());
        slider.value = GetComponent<VillageHealth>().getHealthPercentage();
    }

    private void OnMouseExit()
    {
        villagePopupHealthCanvas.SetActive(false);
    }
}
