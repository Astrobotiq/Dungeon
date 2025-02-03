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
        }
    }

    public override void ApplyEffect(Grid targetGrid = null) {
        if (_target.gameObject && _target.GridObject && _target.GridObject.GetComponent<IDamagable>()) {
            _target.GridObject.GetComponent<IDamagable>().Damage(DamageAmount);
        }

        if (_target.gameObject && _target.GridObject && _target.GridObject.GetComponent<IPushable>()) {
            _target.GridObject.GetComponent<IPushable>().Push( PlayerManager.Instance.GetSelectedPlayer().GetComponent<Player>().Grid.transform.position);
        }
        
        Destroy(gameObject);
    }
}
