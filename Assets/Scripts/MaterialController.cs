using UnityEngine;

public class MaterialController
{
    bool canChangeAlpha = false;
    private Renderer objRenderer;
    private MaterialPropertyBlock propertyBlock;

    private float alpha = 0.5f;

    public MaterialController(Renderer renderer, float _alpha)
    {
        objRenderer = renderer;
        alpha = _alpha;
        if (objRenderer != null)
        {
            propertyBlock = new MaterialPropertyBlock();
        }
    }

    public void SetMaterialWalkable()
    {
        if (objRenderer != null && propertyBlock != null)
        {
            // Mevcut property block'u al
            objRenderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetFloat("_LerpScale", 0.7f);
            objRenderer.SetPropertyBlock(propertyBlock);
        } 
    }

    public void SetMaterialDefault()
    {
        if (objRenderer != null && propertyBlock != null)
        {
            // Mevcut property block'u al
            objRenderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetFloat("_LerpScale", 0);
            objRenderer.SetPropertyBlock(propertyBlock);
        }
    }

    public void SetOutlineScale()
    {
        if (objRenderer != null && propertyBlock != null)
        {
            // Mevcut property block'u al
            objRenderer.GetPropertyBlock(propertyBlock);
            
            propertyBlock.SetFloat("_Size", 1.05f);
            
            objRenderer.SetPropertyBlock(propertyBlock);
        }
    }

    public void ResetOutlineScale()
    {
        if (objRenderer != null && propertyBlock != null)
        {
            // Mevcut property block'u al
            objRenderer.GetPropertyBlock(propertyBlock);
            
            propertyBlock.SetFloat("_Size", 1f);
            
            objRenderer.SetPropertyBlock(propertyBlock);
        }
    }
}
