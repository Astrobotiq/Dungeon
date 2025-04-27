using UnityEngine;

public class WİzardBall : ISkillEffect
{
    private Grid _targetGrid;
    
    [SerializeField]
    private int damageAmount = 3;
    
    [SerializeField] 
    private SoundManager soundManager;

    public float EnemyWizardHitSoundVolume = 1f;
    public override void StartMoving(Grid targetGrid)
    {
        _targetGrid = targetGrid;
        
        soundManager = GameObject.FindWithTag("SoundManager").GetComponent<SoundManager>();
        
        ApplyEffect(targetGrid);
    }

    public override void ApplyEffect(Grid targetGrid)
    {
        if (_targetGrid.GridObject != null)
        {
            _targetGrid.GridObject.GetComponent<IHealth>().TakeDamage(damageAmount);
            soundManager.PlaySound(SoundType.EnemyWizardSkillHitSound,EnemyWizardHitSoundVolume);
        }
        
        Timed.Run((() => Destroy(gameObject)), 3f);
    }
}
