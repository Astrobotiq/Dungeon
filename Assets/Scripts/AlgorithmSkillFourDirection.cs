using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AlgorithmSkillFourDirection
{
    private HashSet<Vector3> grids = new HashSet<Vector3>();
    
    public HashSet<Vector3> startAlgorithm(Grid input_grid, int distance,bool isLikeMortar)
    {
        AlgorithmV1(input_grid,grids,distance,isLikeMortar);
        return grids;
    }

    public void AlgorithmV1(Grid input_grid, HashSet<Vector3>input_set, int input_distance, bool isLikeMortar) {
        Debug.Log(input_grid.gameObject.transform.position);
        GridManager gridManagerInstance = GridManager.Instance;
        Vector3 location = input_grid.gameObject.transform.position;
        
        
        //Buradaki hatayı çözdüm
        int minXCanGo=(int)location.x - input_distance; 
        int maxXCanGo=(int)location.x + input_distance;
        int minZCanGo=(int)location.z - input_distance;
        int maxZCanGo=(int)location.z + input_distance;
        

        if (maxXCanGo > 7) maxXCanGo = 7;
        if (minXCanGo < 0) minXCanGo = 0;
        if (maxZCanGo > 7) maxZCanGo = 7;
        if (minZCanGo < 0) minZCanGo = 0;
        
        if (!(location.x == 7)) {
            for (int i = (int)location.x; i <= maxXCanGo; i++) {
                Grid temp_grid = gridManagerInstance.getGridFromLocation(new Vector3(i, location.y, location.z));
                if (temp_grid.GridObject == null) {
                    input_set.Add(new Vector3(i, location.y, location.z));
                }
                else if (temp_grid.GridObject != null & isLikeMortar) {
                    continue;
                }
                else if (temp_grid.GridObject != null  && temp_grid.GridObject != input_grid.GridObject & isLikeMortar == false) {
                    break;
                }
                //grid blockedsa ve havan değilse break yap
                //grid blocksa ve havansa continue yap
            }
        }
        if (!(location.x == 0)) {
            for (int i = (int)location.x; i >= minXCanGo; i--) {
                Grid temp_grid = gridManagerInstance.getGridFromLocation(new Vector3(i, location.y, location.z));
                if (temp_grid.GridObject == null){ //Şu an sadece block mu değil mi check ediyor
                    input_set.Add(new Vector3(i, location.y, location.z));
                }
                else if (temp_grid.GridObject != null & isLikeMortar) {
                    continue;
                }
                else if (temp_grid.GridObject != null  && temp_grid.GridObject != input_grid.GridObject & isLikeMortar == false) {
                    break;
                }
            }
        }
        if (!(location.z == 7)) {
            for (int i = (int)location.z; i <= maxZCanGo; i++) {
                Grid temp_grid = gridManagerInstance.getGridFromLocation(new Vector3(location.x, location.y, i));
                if (temp_grid.GridObject == null){ //Şu an sadece block mu değil mi check ediyor
                    input_set.Add(new Vector3(location.x, location.y, i));
                }
                else if (temp_grid.GridObject != null & isLikeMortar) {
                    continue;
                }
                else if (temp_grid.GridObject != null  && temp_grid.GridObject != input_grid.GridObject & isLikeMortar == false) {
                    break;
                }
            }
        }
        if (!(location.z == 0)) {
            for (int i = (int)location.z; i >= minZCanGo; i--) {
                Grid temp_grid = gridManagerInstance.getGridFromLocation(new Vector3(location.x, location.y, i));
                if (temp_grid.GridObject == null){ //Şu an sadece block mu değil mi check ediyor
                    input_set.Add(new Vector3(location.x, location.y, i));
                }
                else if (temp_grid.GridObject != null & isLikeMortar) {
                    continue;
                }
                else if (temp_grid.GridObject != null  && temp_grid.GridObject != input_grid.GridObject & isLikeMortar == false) {
                    break;
                }
            }
        }

        if (isLikeMortar) {
            for (int i = -1; i <= 1; i++) { // This parts removes the grids which are 1 distance away grids because mortar cannot fire to 1 distance away tiles
                if (i == 0) {
                    continue;
                }

                input_set.Remove(new Vector3(location.x + i, location.y, location.z));
                input_set.Remove(new Vector3(location.x , location.y, location.z + i));
            }
        }
        
        Debug.Log(input_set.Count);
    }
}
