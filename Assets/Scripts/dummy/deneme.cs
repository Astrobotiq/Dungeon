using System.Collections;
using MoreMountains.Tools;
using UnityEngine;

public class deneme : MonoBehaviour //Direkt bu SİLİNECEK
{
    public GameObject enemy;
    public GameObject orbitingSphere;
    public GameObject player;
    void Start() {
        //GetComponent<RotateSkillScript>().ApplyEffect2(temp);
        StartCoroutine(mainCoroutine());
    }
    
    
    IEnumerator mainCoroutine() {
        //StartCoroutine(openthunder(temp));
        //StartCoroutine(setOrbitingThuder());
        yield break;
    }
    
    /*
    IEnumerator openthunder(GameObject input) {
        new WaitForSeconds(120f);
        Debug.Log("açıyorum pngyi");
        input.GetComponent<Enemy>().openThunderPng();
        new WaitForSeconds(120f);
        Debug.Log("açıyorum pngyi");
        input.GetComponent<Enemy>().closeThunderPng();
        yield return new WaitForSeconds(2f);
        yield break;
    }*/

    /*IEnumerator setOrbitingThuder() {
        Vector3 temp = new Vector3(100, 100, 100);
        GameObject sphereRef=Instantiate(orbitingSphere, this.temp.transform.position, this.temp.transform.rotation);
        MMAutoRotate mmRef =sphereRef.GetComponent<MMAutoRotate>();
        mmRef.Orbiting = true; //burada player etrafında dönsün diye MMAutoRotate scrpitindeki orbiting değerini true çekip
        mmRef.OrbitCenterTransform = player.transform; // burada da aynı scriptin orbit centerını player'a eşitledim
        yield return null;
    }*/
}
