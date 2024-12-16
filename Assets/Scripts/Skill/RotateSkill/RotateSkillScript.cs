using System;
using Unity.VisualScripting;
using UnityEngine;

public class RotateSkillScript : MonoBehaviour {
    //Bunu atılan bir şey mi yoksa tıklanan bir şey mi yapacağımızdan emin olamadım diye 2 tane yazdım
    
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int speed;
    [SerializeField] private GameObject PlayerRef;
    

    private void Start() { //Bu kısımda player ref ve diğer şeyleri set edip ardından playerdan başlayacak şekilde Instanciate ettim
        PlayerRef = PlayerManager.Instance.GetSelectedPlayer();
        spawnPoint = PlayerRef.transform;
        Instantiate(this, spawnPoint.position, spawnPoint.rotation); 
    }

    public void Update() { // Forward yönünde hareketi için
        gameObject.GetComponent<Rigidbody>().linearVelocity = speed * PlayerRef.transform.forward;
    }

    void OnCollisionEnter(Collision collision) { // Enemy scripti içeren bir şeye çarparsa onun game objesini 90 derece döndürüyor
        if (collision.gameObject.GetComponent<Enemy>() != null) {
            collision.gameObject.transform.Rotate(Vector3.right,90);
        }
    }
    
    
    //Bu tıklamalıyıp içine gameobject vererek yapılan
    /*
     public void RotateObject(GameObject input_gameobject) {
        input_gameobject.transform.Rotate(Vector3.right, 90);
    }
    */
}
