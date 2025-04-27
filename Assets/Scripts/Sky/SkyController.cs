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

    

    public IEnumerator ChangeSky(float fromColor, float toColor,float fromStar, float toStar, float duration)
    {
        if (objRenderer != null && propertyBlock != null)
        {
            objRenderer.GetPropertyBlock(propertyBlock);

            float elapsed = 0f;

            while (elapsed < duration)
            {
                float t = elapsed / duration;
                float skyValue = Mathf.Lerp(fromColor, toColor, t);
                float starValue = Mathf.Lerp(fromStar, toStar, t);
                
                propertyBlock.SetFloat("_Base_Color_Gradient_Power", skyValue);
                propertyBlock.SetFloat("_Glitter_Offset",starValue);
                objRenderer.SetPropertyBlock(propertyBlock);

                elapsed += Time.deltaTime;
                yield return null;
            }

            // Final value to ensure accuracy
            propertyBlock.SetFloat("_Base_Color_Gradient_Power", toColor);
            propertyBlock.SetFloat("_Glitter_Offset", toStar);
            objRenderer.SetPropertyBlock(propertyBlock);
        }
    }

}
