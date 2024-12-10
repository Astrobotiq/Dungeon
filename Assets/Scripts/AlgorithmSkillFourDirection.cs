using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AlgorithmSkillFourDirection
{
    private HashSet<Vector3> grids = new HashSet<Vector3>();
    
    public HashSet<Vector3> startAlgorithm(Grid input_grid, int distance)
    {
        AlgorithmV1(input_grid,grids,distance);
        return grids;
    }

    public void AlgorithmV1(Grid input_grid, HashSet<Vector3>input_set, int input_distance)
    {
        GridManager gridManagerInstance = GridManager.Instance;
        Vector3 location = input_grid.gameObject.transform.position;
        int minXCanGo=0; int maxXCanGo=0;
        int minZCanGo=0; int maxZCanGo=0;
        

        if ((int)location.x + input_distance > 7) maxXCanGo = 7;
        if ((int)location.x - input_distance < 0) minXCanGo = 0;
        if ((int)location.z + input_distance > 7) maxZCanGo = 7;
        if ((int)location.z + input_distance < 0) minZCanGo = 0;
        
        if (!(location.x == 7))
        {
            for (int i = (int)location.x; i <= maxXCanGo; i++)
            {
                Grid temp_grid = gridManagerInstance.getGridFromLocation(new Vector3(i, location.y, location.z));
                temp_grid.checkIsThereObjectUpside();
                if (temp_grid.getIsAvailable()){ //Şu an sadece block mu değil mi check ediyor, havan durumu da eklenince alttaki comment gibi yapılacak
                    input_set.Add(new Vector3(i, location.y, location.z));
                }
                //grid blockedsa ve havan değilse break yap
                //grid blocksa ve havansa continue yap
            }
        }
        if (!(location.x == 0))
        {
            for (int i = (int)location.x; i >= minXCanGo; i--)
            {
                Grid temp_grid = gridManagerInstance.getGridFromLocation(new Vector3(i, location.y, location.z));
                temp_grid.checkIsThereObjectUpside();
                if (temp_grid.getIsAvailable()){ //Şu an sadece block mu değil mi check ediyor
                    input_set.Add(new Vector3(i, location.y, location.z));
                }
            }
        }
        if (!(location.z == 7))
        {
            for (int i = (int)location.z; i <= maxZCanGo; i++)
            {
                Grid temp_grid = gridManagerInstance.getGridFromLocation(new Vector3(i, location.y, location.z));
                temp_grid.checkIsThereObjectUpside();
                if (temp_grid.getIsAvailable()){ //Şu an sadece block mu değil mi check ediyor
                    input_set.Add(new Vector3(location.x, location.y, i));
                }
            }
        }
        if (!(location.z == 0))
        {
            for (int i = (int)location.z; i >= minZCanGo; i--)
            {
                Grid temp_grid = gridManagerInstance.getGridFromLocation(new Vector3(i, location.y, location.z));
                temp_grid.checkIsThereObjectUpside();
                if (temp_grid.getIsAvailable()){ //Şu an sadece block mu değil mi check ediyor
                    input_set.Add(new Vector3(location.x, location.y, i));
                }
            }
        }
    }
}
