using System.Collections;
using UnityEngine;

public class SwordSwingSkill : ISkillEffect {
    [SerializeField] float swingDuration = 1.5f;
    public int DamageAmount;
    [SerializeField] Grid _target;
    [SerializeField] private GameObject _player;
    
    [SerializeField] 
    private SoundManager soundManager;

    public float SwordSwingSoundVolume = 1f;
    public float SwordHitSoundVolume = 1f;
    
    public override void StartMoving(Grid targetGrid) {
        _player = PlayerManager.Instance.GetSelectedPlayer();
        _target = targetGrid;
        gameObject.transform.rotation = _player.transform.rotation;
        
        soundManager = GameObject.FindWithTag("SoundManager").GetComponent<SoundManager>();
        soundManager.PlaySound(SoundType.SwordSwing,SwordSwingSoundVolume);
        
        Timed.Run((() => ApplyEffect()), swingDuration);
    }

    public override void ApplyEffect(Grid targetGrid = null)
    {
        if (_target.gameObject && _target.GridObject && _target.GridObject.GetComponent<IDamagable>()) {
            _target.GridObject.GetComponent<IDamagable>().Damage(DamageAmount, true);
            
            soundManager.PlaySound(SoundType.SwordHit,SwordHitSoundVolume);
        }
            
        if (_target.gameObject && _target.GridObject && _target.GridObject.GetComponent<IPushable>()) {
            _target.GridObject.GetComponent<IPushable>().Push( PlayerManager.Instance.GetSelectedPlayer().GetComponent<Player>().Grid.transform.position);
        }
        
        /*if (_target.gameObject && _target.GridObject && _target.GridObject.GetComponent<IHealth>()) {
            _target.GridObject.GetComponent<IHealth>().CheckDeath();
        }*/
            
        Destroy(gameObject);
    }
}
