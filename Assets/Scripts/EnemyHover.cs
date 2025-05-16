using UnityEngine;
using UnityEngine.UI;

public class EnemyHover : MonoBehaviour
{
    [SerializeField] 
    private GameObject enemyPopupHealthCanvas;

    public bool inAttackPreview = false;
    
    private void OnMouseEnter()
    {
        if(enemyPopupHealthCanvas==null)
        {
            Debug.Log("Enemy Popup Health Canvas assign edilmemiştir");
            return;
        }
        
        if(inAttackPreview)
            return;
        
        enemyPopupHealthCanvas.SetActive(true);
        
        enemyPopupHealthCanvas.transform.LookAt(Camera.main.transform.position, Vector3.up);

        Slider slider = enemyPopupHealthCanvas.transform.GetChild(0).gameObject.GetComponent<Slider>();
        //Debug.Log("Enemy Hover Health test health: " + GetComponent<EnemyHealth>().getHealthPercentage());
        slider.value = GetComponent<EnemyHealth>().getHealthPercentage();
    }

    private void OnMouseExit()
    {
        enemyPopupHealthCanvas.SetActive(false);
    }
}
