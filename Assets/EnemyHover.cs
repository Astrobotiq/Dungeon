using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHover : MonoBehaviour
{
    private GameObject EnemyPopupHealthCanvas;
    private void OnMouseEnter()
    {
        GameObject EnemyPopupHealthCanvas = gameObject.transform.GetChild(3).gameObject;
        EnemyPopupHealthCanvas.SetActive(true);
        
        EnemyPopupHealthCanvas.transform.LookAt(Camera.main.transform.position, Vector3.up);

        Slider slider = EnemyPopupHealthCanvas.transform.GetChild(0).gameObject.GetComponent<Slider>();
        Debug.Log("Enemy Hover Health test health: " + GetComponent<EnemyHealth>().getHealthPercentage());
        slider.value = GetComponent<EnemyHealth>().getHealthPercentage();
    }

    private void OnMouseExit()
    {
        GameObject EnemyPopupHealthCanvas = gameObject.transform.GetChild(3).gameObject;
        EnemyPopupHealthCanvas.SetActive(false);
    }
}
