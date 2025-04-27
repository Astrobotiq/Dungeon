using System.Collections;
using UnityEngine;

public class SkyController
{
    private Renderer objRenderer;
    private MaterialPropertyBlock propertyBlock;
    
    public SkyController(Renderer renderer)
    {
        objRenderer = renderer;
        if (objRenderer != null)
        {
            propertyBlock = new MaterialPropertyBlock();
        }
    }

    

    public IEnumerator ChangeSky(float from, float to, float duration)
    {
        if (objRenderer != null && propertyBlock != null)
        {
            objRenderer.GetPropertyBlock(propertyBlock);

            float elapsed = 0f;

            while (elapsed < duration)
            {
                float t = elapsed / duration;
                float skyValue = Mathf.Lerp(from, to, t);
                Debug.Log($"SkyValue : {skyValue}");

                propertyBlock.SetFloat("_Base_Color_Gradient_Power", skyValue);
                objRenderer.SetPropertyBlock(propertyBlock);

                elapsed += Time.deltaTime;
                yield return null;
            }

            // Final value to ensure accuracy
            propertyBlock.SetFloat("_Base_Color_Gradient_Power", to);
            objRenderer.SetPropertyBlock(propertyBlock);
        }
    }

}
