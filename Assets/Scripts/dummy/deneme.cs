using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Tools;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class deneme : MonoBehaviour //Direkt bu SİLİNECEK
{
    public GameObject enemy;
    public GameObject orbitingSphere;
    public GameObject player;

    public Grid tempGrid;
    public int distance;
    public bool isMortar;

    private GridManager gridManager;
    void Start() {
        gridManager = GridManager.Instance;
        //GetComponent<RotateSkillScript>().ApplyEffect2(temp);
        StartCoroutine(mainCoroutine());
    }
    
    
    IEnumerator mainCoroutine() {
        //StartCoroutine(openthunder(temp));
        //StartCoroutine(setOrbitingThuder());
        /*yield return new WaitForSeconds(1);
        tempGrid = GridManager.Instance.getGridFromLocation(new Vector3(4, 0, 4));
        StartCoroutine(checkFourDirectionAlg(tempGrid, distance, isMortar));
        */
        //StartCoroutine(testingDash(A));
        yield break;
    }

    /*
     IEnumerator checkFourDirectionAlg(Grid input_grid, int distance, bool isMortar)
    {
        AlgorithmSkillFourDirection alg = new AlgorithmSkillFourDirection();
        HashSet<Vector3> naber = alg.startAlgorithm(input_grid, distance, isMortar);
        foreach (Vector3 vec3 in naber) {
            Debug.Log(vec3);
            var grid = gridManager.getGridFromLocation(vec3);
            grid.MaterialController.SetMaterialWalkable();
        }
        

        yield return null;
    }
    */
    
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
    }
    */

    /*
     IEnumerator setOrbitingThuder() {
        Vector3 temp = new Vector3(100, 100, 100);
        GameObject sphereRef=Instantiate(orbitingSphere, this.temp.transform.position, this.temp.transform.rotation);
        MMAutoRotate mmRef =sphereRef.GetComponent<MMAutoRotate>();
        mmRef.Orbiting = true; //burada player etrafında dönsün diye MMAutoRotate scrpitindeki orbiting değerini true çekip
        mmRef.OrbitCenterTransform = player.transform; // burada da aynı scriptin orbit centerını player'a eşitledim
        yield return null;
    }
    */

    IEnumerator testingDash(List<Grid> inputLazımdı) { //Bununla çalıştıramadım
        List<List<Grid>> naber = dashListPreparation(inputLazımdı);
        int count = 1;
        foreach (List<Grid> ilkKabuk in naber) {
            foreach (Grid gridcik in ilkKabuk) {
                Debug.Log("şu an şu listede: " + count + "ve şu elemandayım: " + gridcik.transform.position);
            }
            count++;
        }
        yield return null;
        
        /*List<Grid> givenPath = new List<Grid>();

        Grid grid = new Grid();
        grid.transform.position = new UnityEngine.Vector3(0, 5, 0);
        givenPath.Add(grid);

        grid = new Grid();
        grid.transform.position = new UnityEngine.Vector3(1, 5, 0);
        givenPath.Add(grid);

        grid = new Grid();
        grid.transform.position = new UnityEngine.Vector3(2, 5, 0);
        givenPath.Add(grid);

        grid = new Grid();
        grid.transform.position = new UnityEngine.Vector3(3, 5, 0);
        givenPath.Add(grid);

        grid = new Grid();
        grid.transform.position = new UnityEngine.Vector3(3, 5, 1);
        givenPath.Add(grid);

        grid = new Grid();
        grid.transform.position = new UnityEngine.Vector3(3, 5, 2);
        givenPath.Add(grid);

        grid = new Grid();
        grid.transform.position = new UnityEngine.Vector3(4, 5, 2);
        givenPath.Add(grid);

        grid = new Grid();
        grid.transform.position = new UnityEngine.Vector3(4, 5, 3);
        givenPath.Add(grid);

        List<List<Grid>> naber = dashListPreparation(givenPath);
        int count = 1;
        foreach (List<Grid> ilkKabuk in naber) {
            foreach (Grid gridcik in ilkKabuk) {
                Debug.Log("şu an şu listede: " + count + "ve şu elemandayım: " + gridcik.transform.position);
            }
            count++;
        }
        yield return null;*/
    }
    public List<List<Grid>> dashListPreparation(List<Grid> input_path) { // I tried it on paper and it worked but I couldn't do the testing of it 
        bool same_X = false; // İf they has the same x value
        bool same_Z = false; // İf they has the same z value
        List<List<Grid>> listHolder = new List<List<Grid>>();
        List<Grid> tempList = new List<Grid>();
        
        if (input_path.Count < 2) { // Just for error handling
            return null;
        }
        
        if (input_path.Count == 2) { // İf it has 2 values we don't care about its direction. One way or another they are at the same direction. So just put them on temp, add temp to listholder and sent back
            tempList.Add(input_path[0]);
            tempList.Add(input_path[1]);
            listHolder.Add(tempList);
        }
        else {
            for (int i = 0; i<input_path.Count-1; i++) { // By starting from the start it goes to sondan bir onceki
                if (input_path[i].transform.position.x == input_path[i+1].transform.position.x) { // we check the current one and the next one has same x value or not
                    same_X = true;
                }
                else {
                    same_Z = true;
                }
                
                // One way or another, same_x and same_z value only be true when there is a change in direction and this if statement becomes false. Btw I mean a direction change like (1,0), (2,0), (3,0), (3,1)
                if (!(same_X & same_Z)) { // When only one of them is true and the other one is false, this if statement becomes true and it states that we don't have a change so continue to add the same temp list
                    tempList.Add(input_path[i]);
                }
                else { // If you come here, it states that now both bool values are true so we need to open a new list and add to it
                    tempList.Add(input_path[i]);
                    listHolder.Add(tempList);
                    tempList = new List<Grid>();
                    same_X = false;
                    same_Z = false;
                }
            }
            
            // This block is for to handle last index. The previous for loop only able to look until sondan bir onceki index.
            // The logic is I compare sondan 3. index ve sonuncu index and I also look for our bool values which still holds the compare between sondan bir onceki ve sonuncu değer
            // eğer same_x ve sondan 3. index ile sonuncu arasındaki x değeri karşılaştırması olmusuz ise demektir ki bizim son değer sondan 3. değerin çaprazında. Bunun z versiyonu da else if durumundaki. else ise sondan 3 değer de aynı x veya z yönünde ise çalışır
            if(!(same_X & (input_path[input_path.Count-3].transform.position.x == input_path[input_path.Count-1].transform.position.x))) {
                listHolder.Add(tempList);
                tempList = new List<Grid>();
                tempList.Add(input_path[input_path.Count-1]);
                listHolder.Add(tempList);
                same_X = false;
                same_Z = false;
            }
            else if(!(same_Z & (input_path[input_path.Count-3].transform.position.z == input_path[input_path.Count-1].transform.position.z))) {
                listHolder.Add(tempList);
                tempList = new List<Grid>();
                tempList.Add(input_path[input_path.Count-1]);
                listHolder.Add(tempList);
                same_X = false;
                same_Z = false;
            }
            else {
                tempList.Add(input_path[input_path.Count-1]);
            }
        }
        return listHolder;
    }
}
