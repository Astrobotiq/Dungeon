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

    public float ButtonHoverSoundVolume = 1f;

    private void Start()
    {
        tempRectTransformLocalScale = gameObject.GetComponent<RectTransform>().localScale;
        
        if (soundManager == null)
        {
            Debug.Log("Soundmanager'ım yok, ben ButtonHover ve şunun içindeki " + gameObject.name);
            soundManager = GameObject.FindWithTag("SoundManager").GetComponent<SoundManager>();
        }
    }

    public void OnPointerEnter()
    {
        if(ReferedButton==null)
        {
            ReferedButton = transform.gameObject;
            Debug.Log(ReferedButton.gameObject.name + " adındaki objenin Refered Button assign edilmemiş");
            return;
        }
        
        soundManager.PlaySound(SoundType.ButtonHoverSound, ButtonHoverSoundVolume);
        
        tempRectTransformLocalScale = gameObject.GetComponent<RectTransform>().localScale;
        gameObject.GetComponent<RectTransform>().localScale = new Vector3(
            tempRectTransformLocalScale.x + HoverScaleValue, 
            tempRectTransformLocalScale.y + HoverScaleValue,
            tempRectTransformLocalScale.z );
    }

    public void OnPointerExit()
    {
        gameObject.GetComponent<RectTransform>().localScale = tempRectTransformLocalScale;
        
        soundManager.PlaySound(SoundType.ButtonHoverSound, ButtonHoverSoundVolume);
    }
}
