using System;
using DG.Tweening;
using MoreMountains.Tools;
using UnityEngine;

public class ThunderSkillScript : ISkillEffect
{
    [SerializeField] private float duration;
    [SerializeField] private int damageAmount;
    
    public override void StartMoving(Grid targetGrid) {
        transform.DOMove(targetGrid.gameObject.transform.position, duration).
            OnComplete(() => {
                ApplyEffect(targetGrid);
            });
    }

    public override void ApplyEffect(Grid targetGrid) {
        if (targetGrid.GridObject.GetComponent<IDamagable>() != null)
        {
            targetGrid.GridObject.GetComponent<IDamagable>().Damage(damageAmount);
        }
        
        targetGrid.GridObject.GetComponent<Enemy>().openThunderPng();
    }
}

/*

    [SerializeField] private GameObject thunderSprite;
    [SerializeField] private GameObject orbitingSphere;
    [SerializeField] private GameObject playerRef;

    public void Start() {
        playerRef = PlayerManager.Instance.GetSelectedPlayer();
    }

    public void strikeLightning(GameObject input_gameobject) {
        
        
        //Enemynin kafada yıldırım işareti çıksın diye burada "lightningPNG" instanciate ediyorum
        Transform enemyTransform = input_gameobject.transform;
        enemyTransform.position = new Vector3(enemyTransform.position.x + 0.3f, enemyTransform.position.y + 2.1f, enemyTransform.position.z - 0.1f);
        Instantiate(thunderSprite, enemyTransform.position, enemyTransform.rotation);

        input_gameobject.GetComponent<Enemy>(); //bunu dummy olarak bırakıyorum
        // şu an yok ama enemyyi tam yazdığımızda alttaki gibi bir şey olacak sanırım
        // input_gameobject.GetComponent<Enemy>().setIsStunned(bool input_bool);
    }
*/