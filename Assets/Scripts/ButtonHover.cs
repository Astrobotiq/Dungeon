using System;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonHover : MonoBehaviour
{
    [SerializeField] private GameObject ReferedButton;

    [SerializeField] private float HoverScaleValue = 0.15f;

    private Vector3 tempRectTransformLocalScale;
    
    [SerializeField] 
    private SoundManager soundManager;

    public float ButtonHoverSoundVolume = 0.5f;

    private void Start()
    {
        tempRectTransformLocalScale = gameObject.GetComponent<RectTransform>().localScale;
        
        if (soundManager == null)
        {
            Debug.Log("Soundmanager'ım yok, ben ButtonHover ve şunun içindeki " + gameObject.name);
            soundManager = GameObject.FindWithTag("SoundManager").GetComponent<SoundManager>();
        }
    }

    public void MakeButtonSizeNormal() {
        gameObject.GetComponent<RectTransform>().localScale = tempRectTransformLocalScale;
    }

    public void OnPointerEnter()
    {
        if(ReferedButton==null)
        {
            ReferedButton = transform.gameObject;
            Debug.Log(ReferedButton.gameObject.name + " adındaki objenin Refered Button assign edilmemiş");
            return;
        }
        
        if (soundManager == null)
        {
            Debug.Log("Soundmanager'ım yok, ben ButtonHover ve şunun içindeki " + gameObject.name);
            soundManager = GameObject.FindWithTag("SoundManager").GetComponent<SoundManager>();
        }
        
        soundManager.PlaySound(SoundType.ButtonHoverSound, ButtonHoverSoundVolume);
        
        Vector3 temp = gameObject.GetComponent<RectTransform>().localScale;
        gameObject.GetComponent<RectTransform>().localScale = new Vector3(
            temp.x + HoverScaleValue, 
            temp.y + HoverScaleValue,
            temp.z );
    }

    public void OnPointerExit()
    {
        gameObject.GetComponent<RectTransform>().localScale = tempRectTransformLocalScale;
        
        soundManager.PlaySound(SoundType.ButtonHoverSound, ButtonHoverSoundVolume);
    }
}
