using System.Collections;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UIElements;

public class SmiteSwordSwingSkill : ISkillEffect { //Bir yön skill GO particleların yönü konusunda bir problemi olduğunun farkındayım halledeceğim
    [SerializeField] float swingDuration = 1.5f;
    public int DamageAmount;
    public float DamageFallOf;
    [SerializeField] Grid _target;
    [SerializeField] GameObject _player;
    
    [SerializeField] 
    private SoundManager soundManager;

    public float SmiteSwordSwingSoundVolume = 1f;
    public float SwordHitSoundVolume = 1f;
    
    public override void StartMoving(Grid targetGrid) {
        _player = PlayerManager.Instance.GetSelectedPlayer();
        _target = targetGrid;
        gameObject.transform.rotation = _player.transform.rotation;
        
        soundManager = GameObject.FindWithTag("SoundManager").GetComponent<SoundManager>();
        soundManager.PlaySound(SoundType.SmiteSwordSwing,SmiteSwordSwingSoundVolume);
        
        Timed.Run((() => ApplyEffect()), swingDuration);
    }

    public override void ApplyEffect(Grid targetGrid = null) {
        //Hasar alacak ana düşman için
        if (_target.gameObject && _target.GridObject && _target.GridObject.GetComponent<IDamagable>())
        {
            _target.GridObject.GetComponent<IDamagable>().Damage(DamageAmount, true);
            soundManager.PlaySound(SoundType.SwordHit,SwordHitSoundVolume);
        }
        if (_target.gameObject && _target.GridObject && _target.GridObject.GetComponent<IPushable>())
        {
            _target.GridObject.GetComponent<IPushable>().Push( PlayerManager.Instance.GetSelectedPlayer().GetComponent<Player>().Grid.transform.position);
        }
        
        
        Vector3 targetGridPos = _target.gameObject.transform.position;
        
        //Hasar alacak arkadaki düşmanlar aynı uygulamanın yapılmasıiçin bu if else kısmı
        if (_player.gameObject.transform.rotation == Quaternion.Euler(0, 0, 0)) { // player looking +z (yukari)
            Grid nextGrid = GridManager.Instance.getGridFromLocation(new Vector3(targetGridPos.x, targetGridPos.y, targetGridPos.z + 1));
            
            if (nextGrid.gameObject && nextGrid.GridObject && nextGrid.GridObject.GetComponent<IDamagable>()) {
                nextGrid.GridObject.GetComponent<IDamagable>().Damage((int) (DamageAmount * DamageFallOf));
                soundManager.PlaySound(SoundType.SwordHit,SwordHitSoundVolume);
            }
        }
        else if (_player.gameObject.transform.rotation == Quaternion.Euler(0, 90, 0)) { // player looking +x (sağa)
            Vector3 tempTransform = _target.transform.position;
            Grid nextGrid = GridManager.Instance.getGridFromLocation(new Vector3(targetGridPos.x + 1, targetGridPos.y, targetGridPos.z));
            
            if (nextGrid.gameObject && nextGrid.GridObject && nextGrid.GridObject.GetComponent<IDamagable>()) {
                nextGrid.GridObject.GetComponent<IDamagable>().Damage((int) (DamageAmount * DamageFallOf));
                soundManager.PlaySound(SoundType.SwordHit,SwordHitSoundVolume);
            }
        }
        else if (_player.gameObject.transform.rotation == Quaternion.Euler(0, 180, 0)) { // player looking -z (arkaya)
            Vector3 tempTransform = _target.transform.position;
            Grid nextGrid = GridManager.Instance.getGridFromLocation(new Vector3(targetGridPos.x, targetGridPos.y, targetGridPos.z - 1));
            
            if (nextGrid.gameObject && nextGrid.GridObject && nextGrid.GridObject.GetComponent<IDamagable>()) {
                nextGrid.GridObject.GetComponent<IDamagable>().Damage((int) (DamageAmount * DamageFallOf));
                soundManager.PlaySound(SoundType.SwordHit,SwordHitSoundVolume);
            }
        }
        else if (_player.gameObject.transform.rotation == Quaternion.Euler(0, 270, 0)) { // player looking -x (sola)
            Vector3 tempTransform = _target.transform.position;
            Grid nextGrid = GridManager.Instance.getGridFromLocation(new Vector3(targetGridPos.x - 1, targetGridPos.y,targetGridPos.z));
            
            if (nextGrid.gameObject && nextGrid.GridObject && nextGrid.GridObject.GetComponent<IDamagable>()) {
                nextGrid.GridObject.GetComponent<IDamagable>().Damage((int) (DamageAmount * DamageFallOf));
                soundManager.PlaySound(SoundType.SwordHit,SwordHitSoundVolume);
            }
        }
        
        Destroy(gameObject);
    }
}
