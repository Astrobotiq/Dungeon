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

            // Overlay rengini ayarla (örneğin varsayılan kırmızı)
            Color overlayColor = new Color(0, 1, 0, alpha); // İstediğiniz renk ve alpha değeri
            propertyBlock.SetColor("_OverlayColor", overlayColor);

            // Güncellenmiş property block'u geri ata
            objRenderer.SetPropertyBlock(propertyBlock);
        } 
    }

    public void SetMaterialDefault()
    {
        if (objRenderer != null && propertyBlock != null)
        {
            // Mevcut property block'u al
            objRenderer.GetPropertyBlock(propertyBlock);

            // Overlay rengini ayarla (örneğin varsayılan kırmızı)
            Color overlayColor = new Color(1, 1, 1, 1); // İstediğiniz renk ve alpha değeri
            propertyBlock.SetColor("_OverlayColor", overlayColor);

            // Güncellenmiş property block'u geri ata
            objRenderer.SetPropertyBlock(propertyBlock); 
        }
    }

    public void SetOutlineScale()
    {
        if (objRenderer != null && propertyBlock != null)
        {
            // Mevcut property block'u al
            objRenderer.GetPropertyBlock(propertyBlock);
            
            propertyBlock.SetFloat("_Scale", 1.05f);
            
            objRenderer.SetPropertyBlock(propertyBlock);
        }
    }

    public void ResetOutlineScale()
    {
        if (objRenderer != null && propertyBlock != null)
        {
            // Mevcut property block'u al
            objRenderer.GetPropertyBlock(propertyBlock);
            
            propertyBlock.SetFloat("_Scale", 1f);
            
            objRenderer.SetPropertyBlock(propertyBlock);
        }
    }
}
