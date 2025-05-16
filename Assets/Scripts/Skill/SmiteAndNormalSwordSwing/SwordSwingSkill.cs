using System.Collections;
using UnityEngine;

public class SwordSwingSkill : ISkillEffect {
    [SerializeField] float swingDuration = 1.5f;
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
        //bir sonraki gridi su mu diye check et
        var direction = _target.gameObject.transform.position - _player.gameObject.transform.position;
        var newPos = new Vector3(_target.gameObject.transform.position.x + direction.x, 0, _target.gameObject.transform.position.z + direction.z);
        var grid = GridManager.Instance.getGridFromLocation(newPos);
        
        if (grid && grid.GridObject && grid.GridObject.GetComponent<Water>() != null)
        {
            if (_target.gameObject && _target.GridObject && _target.GridObject.GetComponent<IPushable>()) {
                FeelManager.Instance.ShakeCamera();
                _target.GridObject.GetComponent<IPushable>().Push( PlayerManager.Instance.GetSelectedPlayer().GetComponent<Player>().Grid.transform.position);
                soundManager.PlaySound(SoundType.SwordHit,SwordHitSoundVolume);
            }
            return;
            
        }
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
