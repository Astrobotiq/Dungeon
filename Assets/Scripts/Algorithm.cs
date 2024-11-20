using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Vector3 = UnityEngine.Vector3;

public class Algorithm
{
    
    //Yorumlarım
    // 1-Bence bu algoritmaların ortak özelliklerini düşünüp bundan sonra yazacaklarımız için de kolaylık sağlaması adına
    //   bir tane abstract searchAlghorithms class'ı oluşturup bunları da ondan inherit etmen iyi olur.
    // 2-Şuan algoritma çalışmıyor çalışır hale getirirsen çok iyi olur. Bu hafta konuşmadık ama haftaya kadar abstract
    //   ile bu class'ı çalışır hale getirirsen çok iyi olur
    // 3-Bütün algoritma class'ları için diyorum bütün bu renk değiştirme olaylarını falan burada yapmamak daha iyi olur.
    //   Bu class'ların bütün amacı onlara verdiğimiz listeler üzerinden search yapıp geriye hash veya list dönmesi olsun
    //   kod daha temiz olur. herkes herkesin işine elini sokmaz. geriye dönülen liste veya hash ile dönülen yerde iş yapılır.
    // 4-Şimdiden bol kolaylıklar.
    
    private HashSet<Vector3> grids = new HashSet<Vector3>();

    public void startAlgorithm(Grid input_grid, int input_deepeningCount)
    {

        Debug.Log(input_grid.gameObject.transform.position);

        IterativeDeepeningAlgorithmV2(input_grid,grids,input_deepeningCount);

        foreach (Vector3 gridPos in grids)
        {
            Debug.Log("1-gridPos :"+gridPos);
            MaterialController controller = GridManager.Instance.getGridFromLocation(gridPos).gameObject
                .GetComponent<Grid>().MaterialController;
            controller.SetMaterialWalkable();
        }
    }
    private void IterativeDeepeningAlgorithmV2(Grid input_grid, HashSet<Vector3>input_set, int input_deepeningCount){
        calculateNearNodes(input_grid.gameObject.transform.position);
        if(input_deepeningCount==1){
            foreach (Vector3 grid in input_grid.getNearNodes())
            input_set.Add(grid);
        }

        List<Vector3> açılacakNodelar = new List<Vector3>(); //şuanki input_node ne ise onun komşularını tutar
        foreach(Vector3 grid in input_grid.getNearNodes()){ //yakın nodeları stack ve liste ekler
            if(!input_set.Contains(grid)){
                //input_stack.Push(grid);
                açılacakNodelar.Add(grid);
            }
        }

        foreach(Vector3 grid in açılacakNodelar) {  //yukarıdaki listede eklenen bir öncesinde açılmış nodeun komşularını açar
            Debug.Log("2-grid: "+grid);
            if(!input_set.Contains(grid)){
                IterativeDeepeningAlgorithmV2(GridManager.Instance.getGridFromLocation(grid),input_set, input_deepeningCount-1);
            }
            input_set.Add(grid);
        }
    }
    
    private void calculateNearNodes(Vector3 coordinates)
    {
        Vector3 location = coordinates;
        List<Vector3> nearNodes = new List<Vector3>();
        

        if(location.z==0){ // y=0 ise
            if(location.x==0){ //hem x hem y = 0 ise
                nearNodes.Add(new Vector3(location.x,location.y,location.z + 1)); //sağ
                nearNodes.Add(new Vector3(location.x-1,location.y,location.z)); //aşşağı
            }
            else if(location.x==7){ //hem x hem de y 7 ise
                nearNodes.Add(new Vector3(location.x,location.y,location.z+1)); //sağ
                nearNodes.Add(new Vector3(location.x+1,location.y,location.z)); //yukarı
            }
            else{
                nearNodes.Add(new Vector3(location.x,location.y,location.z+1)); //sağ
                nearNodes.Add(new Vector3(location.x+1,location.y,location.z)); //yukarı
                nearNodes.Add(new Vector3(location.x-1,location.y,location.z)); //aşşağı
            }
        }
        else if(location.z==7){ // y=7 ise
            if(location.x==7){ //hem x hem de y 7 ise
                nearNodes.Add(new Vector3(location.x,location.y,location.z-1)); //sol
                nearNodes.Add(new Vector3(location.x+1,location.y,location.z)); //yukarı
            }
            else if(location.x==0){ //hem x=0 y 7 ise
                nearNodes.Add(new Vector3(location.x,location.y,location.z-1)); //sol
                nearNodes.Add(new Vector3(location.x-1,location.y,location.z)); //aşşağı
            }
            else{
                nearNodes.Add(new Vector3(location.x,location.y,location.z-1)); //sol
                nearNodes.Add(new Vector3(location.x+1,location.y,location.z)); //yukarı
                nearNodes.Add(new Vector3(location.x-1,location.y,location.z)); //aşşağı
            }
        }
        else if(location.x==0){ // x=0 ise
            if(location.z==7){ // x=0 y=7 ise
                nearNodes.Add(new Vector3(location.x,location.y,location.z-1)); //sol
                nearNodes.Add(new Vector3(location.x-1,location.y,location.z)); //aşşağı
            }
            else{
                nearNodes.Add(new Vector3(location.x,location.y,location.z+1)); //sağ
                nearNodes.Add(new Vector3(location.x,location.y,location.z-1)); //sol
                nearNodes.Add(new Vector3(location.x-1,location.y,location.z)); //aşşağı
            }
        }
        else if (location.x==7){ // x=7 ise
            if(location.z==0){ // hem x=7 y=0 ise
                nearNodes.Add(new Vector3(location.x,location.y,location.z+1)); //sağ
                nearNodes.Add(new Vector3(location.x+1,location.y,location.z)); //yukarı
            }
            else{
                nearNodes.Add(new Vector3(location.x,location.y,location.z+1)); //sağ
                nearNodes.Add(new Vector3(location.x,location.y,location.z-1)); //sol
                nearNodes.Add(new Vector3(location.x+1,location.y,location.z)); //yukarı
            }
        }
        else{
            nearNodes.Add(new Vector3(location.x,location.y,location.z+1)); //sağ
            nearNodes.Add(new Vector3(location.x,location.y,location.z-1)); //sol
            nearNodes.Add(new Vector3(location.x+1,location.y,location.z)); //yukarı
            nearNodes.Add(new Vector3(location.x-1,location.y,location.z)); //aşşağı
        }

        Debug.Log("3-Location : " + location);
        GridManager.Instance.getGridFromLocation(location).setNearNodes(nearNodes);
        
    }
    
    
    
}
