using UnityEngine;
using UnityEngine.UI;

public class MountainHealth : IHealth
{
    [SerializeField] GameObject particle;
    
    public GameObject MountainPopupHealthCanvas;
    
    public GameObject AttackPreviewHealthCanvas;
    public override void TakeDamage(int damage, bool willPush)
    {
        
        if (currentHealth - damage <= 0)
        {
            EventManager.Instance.InvokeOnMountainDestroyed();
            var particleGO = Instantiate(this.particle, transform, false);
            Destroy(particleGO,1f);
        }
        base.TakeDamage(damage);
        
        if(MountainPopupHealthCanvas==null)
        {
            Debug.Log("Mountain Health Canvas assign edilmemiştir");
            return;
        }
        
        Slider slider = MountainPopupHealthCanvas.transform.GetChild(0).gameObject.GetComponent<Slider>();
        slider.value = GetComponent<MountainHealth>().getHealthPercentage();
    }
}
