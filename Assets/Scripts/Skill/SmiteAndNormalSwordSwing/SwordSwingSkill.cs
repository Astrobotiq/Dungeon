using System.Collections;
using UnityEngine;

public class SwordSwingSkill : ISkillEffect {
    [SerializeField] float swingDuration = 1.5f;
    public int DamageAmount;
    [SerializeField] Grid _target;
    [SerializeField] private GameObject _player;
    
    public override void StartMoving(Grid targetGrid) {
        _player = PlayerManager.Instance.GetSelectedPlayer();
        _target = targetGrid;
        gameObject.transform.rotation = _player.transform.rotation;
        StartCoroutine(Timer());

        IEnumerator Timer()
        {
            yield return new WaitForSeconds(swingDuration);
            ApplyEffect();
            Destroy(gameObject);
        }
    }

    public override void ApplyEffect(Grid targetGrid = null) {
        if (_target.gameObject && _target.GridObject && _target.GridObject.GetComponent<IDamagable>())
        {
            _target.GridObject.GetComponent<IDamagable>().Damage(DamageAmount);
        }
        
        //Burada bi sıkıntı yaşıyorum slash effekti veren particle effectin yönünü ayarlamak konusunda. Ondan geri deönüp bakmam lazım buraya
        //Aynı ayarı smite için olan sword swing için de yapmam lazım
        /*
        ParticleSystem particleSystem = gameObject.GetComponent<ParticleSystem>();
        Transform transformOfPlayer = PlayerManager.Instance.GetSelectedPlayer().transform;
        gameObject.GetComponent<ParticleSystem>().startRotation3D = new Vector3(transformOfPlayer.rotation.x,transformOfPlayer.rotation.y, transformOfPlayer.rotation.z);
        */
    }
}
