using System;
using System.Collections;
using MoreMountains.Tools;
using UnityEngine;

public class ForOrbitingThunder : MonoBehaviour
{
    public GameObject orbitingSphere;
    public GameObject player;
    public void Start() {
        StartCoroutine(setOrbitingThuder());
    }

    IEnumerator setOrbitingThuder()
    {
        if (PlayerManager.Instance.GetSelectedPlayer() == null) {
            Debug.Log("Player manager içindeki selected player boş");
            yield return null;
        }
        
        player = PlayerManager.Instance.GetSelectedPlayer();
        GameObject sphereRef=Instantiate(orbitingSphere, new Vector3(100, 100, 100), new Quaternion(0f,0f,0f,0f));
        MMAutoRotate mmRef =sphereRef.GetComponent<MMAutoRotate>();
        mmRef.Orbiting = true; //burada player etrafında dönsün diye MMAutoRotate scrpitindeki orbiting değerini true çekip
        mmRef.OrbitCenterTransform = player.transform; // burada da aynı scriptin orbit centerını player'a eşitledim
        yield return new WaitForSeconds(240f);
    }
}
