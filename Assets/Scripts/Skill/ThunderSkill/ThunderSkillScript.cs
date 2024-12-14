using System;
using MoreMountains.Tools;
using UnityEngine;

public class ThunderSkillScript :MonoBehaviour
{
    [SerializeField] private GameObject thunderSprite;
    [SerializeField] private GameObject orbitingSphere;
    [SerializeField] private GameObject playerRef;

    public void Start() {
        playerRef = PlayerManager.Instance.GetSelectedPlayer();
    }

    public void strikeLightning(GameObject input_gameobject) {
        //Player etrafında topçuk dönsün diye prefabdaki "OrbitingThunderSphere" instanciate ettim.
        GameObject sphereRef=Instantiate(orbitingSphere, playerRef.transform.position, playerRef.transform.rotation);
        MMAutoRotate mmRef =sphereRef.GetComponent<MMAutoRotate>();
        mmRef.Orbiting = true; //burada player etrafında dönsün diye MMAutoRotate scrpitindeki orbiting değerini true çekip
        mmRef.OrbitCenterTransform = playerRef.transform; // burada da aynı scriptin orbit centerını player'a eşitledim
        
        //Enemynin kafada yıldırım işareti çıksın diye burada "lightningPNG" instanciate ediyorum
        Transform enemyTransform = input_gameobject.transform;
        enemyTransform.position = new Vector3(enemyTransform.position.x + 0.3f, enemyTransform.position.y + 2.1f, enemyTransform.position.z - 0.1f);
        Instantiate(thunderSprite, enemyTransform.position, enemyTransform.rotation);

        input_gameobject.GetComponent<Enemy>(); //bunu dummy olarak bırakıyorum
        // şu an yok ama enemyyi tam yazdığımızda alttaki gibi bir şey olacak sanırım
        // input_gameobject.GetComponent<Enemy>().setIsStunned(bool input_bool);
    }
}
