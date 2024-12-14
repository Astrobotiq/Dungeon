using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class AlgorithmPath { //Bu dursun da bunun yerine "AStarPathFinding" olana bakalım çünkü son yaptığım en güncel o
    public HashSet<Vector3> set = new HashSet<Vector3>();

    public HashSet<Vector3> startAlgorithm(Grid input_grid, Grid input_targetGrid, List<Vector3> neighborGrids) {
        int[] neighborTargetDistance = new int[neighborGrids.Count];
        UnityEngine.Vector3 targetLocation = input_targetGrid.gameObject.transform.position;
        UnityEngine.Vector3 inputLocation = input_grid.gameObject.transform.position;
        
        for(int i = 0;i<neighborGrids.Count;i++) {
            neighborTargetDistance[i]=Math.Abs((int)neighborGrids[i].X - (int)targetLocation.x)
                + Math.Abs((int)neighborGrids[i].Z - (int)targetLocation.z);
        }
        
        return null;
    }
}
