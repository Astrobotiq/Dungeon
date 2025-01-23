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
        //Hasar alacak ana düşman için
        if (_target.gameObject && _target.GridObject && _target.GridObject.GetComponent<IDamagable>())
        {
            _target.GridObject.GetComponent<IDamagable>().Damage(DamageAmount);
        }
        
        Vector3 targetGridPos = _target.gameObject.transform.position;
        
        //Hasar alacak arkadaki düşmanlar aynı uygulamanın yapılmasıiçin bu if else kısmı
        if (_player.gameObject.transform.rotation == Quaternion.Euler(0, 0, 0)) { // player looking +z (yukari)
            Grid nextGrid = GridManager.Instance.getGridFromLocation(new Vector3(targetGridPos.x, targetGridPos.y, targetGridPos.z + 1));
            
            if (nextGrid.gameObject && nextGrid.GridObject && nextGrid.GridObject.GetComponent<IDamagable>()) {
                nextGrid.GridObject.GetComponent<IDamagable>().Damage((int) (DamageAmount * DamageFallOf));
            }
        }
        else if (_player.gameObject.transform.rotation == Quaternion.Euler(0, 90, 0)) { // player looking +x (sağa)
            Vector3 tempTransform = _target.transform.position;
            Grid nextGrid = GridManager.Instance.getGridFromLocation(new Vector3(targetGridPos.x + 1, targetGridPos.y, targetGridPos.z));
            
            if (nextGrid.gameObject && nextGrid.GridObject && nextGrid.GridObject.GetComponent<IDamagable>()) {
                nextGrid.GridObject.GetComponent<IDamagable>().Damage((int) (DamageAmount * DamageFallOf));
            }
        }
        else if (_player.gameObject.transform.rotation == Quaternion.Euler(0, 180, 0)) { // player looking -z (arkaya)
            Vector3 tempTransform = _target.transform.position;
            Grid nextGrid = GridManager.Instance.getGridFromLocation(new Vector3(targetGridPos.x, targetGridPos.y, targetGridPos.z - 1));
            
            if (nextGrid.gameObject && nextGrid.GridObject && nextGrid.GridObject.GetComponent<IDamagable>()) {
                nextGrid.GridObject.GetComponent<IDamagable>().Damage((int) (DamageAmount * DamageFallOf));
            }
        }
        else if (_player.gameObject.transform.rotation == Quaternion.Euler(0, 270, 0)) { // player looking -x (sola)
            Vector3 tempTransform = _target.transform.position;
            Grid nextGrid = GridManager.Instance.getGridFromLocation(new Vector3(targetGridPos.x - 1, targetGridPos.y,targetGridPos.z));
            
            if (nextGrid.gameObject && nextGrid.GridObject && nextGrid.GridObject.GetComponent<IDamagable>()) {
                nextGrid.GridObject.GetComponent<IDamagable>().Damage((int) (DamageAmount * DamageFallOf));
            }
        }

    }
}
