using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField, Range(0.1f,5)]
    float jumpPower = 1;

    [SerializeField] int jumpNumber = 1;

    [SerializeField, Range(0.1f,5)]
    float jumpDuration = 1;

    [SerializeField, Range(0.1f,5)]
    float normalDuration = 1;

    [SerializeField]
    TravelType travelType = TravelType.NORMAL;
    
    //Maybe this function call a coroutine. This will be decided based on can's code
    public void StartMove(Grid startGrid, Grid endGrid)
    {
        AStarPathfinding path = new();
        var grids = path.startAlgorithm(startGrid, endGrid);

        var listlist =dashListPreparation(grids);

        int i = 1;
        foreach (var list in listlist)
        {
            Debug.Log("list index :" + i);
            foreach (var vector in list)
            {
                Debug.Log("asdad" + vector.gameObject.transform.position);
            }

            i++;
        }
        
        //Test
        Debug.Log(grids.Count);

        switch (travelType)
        {
            case TravelType.NORMAL:
                StartCoroutine(NormalTravel(grids));
                break;
            case TravelType.JUMP:
                StartCoroutine(JumpTravel(grids));
                break;
        }
    }

    //Kendi kafamdaki şeyi bulamadım
    IEnumerator NormalTravel(List<Grid> path)
    {   
        InputManager.Instance.canTakeInput = false;
        
        int current = 0;
        while (current + 1 < path.Count)
        {
            Vector3 targetDirection = (path[current+1].transform.position - path[current].transform.position).normalized;

            Vector3 currentDirection = transform.forward;

            float angle = Vector3.SignedAngle(currentDirection, targetDirection, Vector3.up);

            if (Mathf.Abs(angle)>0.01)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

                transform.DORotate(targetRotation.eulerAngles, jumpDuration, RotateMode.Fast);

                yield return new WaitForSeconds(normalDuration);
            }

            var pos = new Vector3(path[current+1].transform.position.x,transform.position.y,path[current+1].transform.position.z);
            transform.DOMove(pos,normalDuration);

            yield return new WaitForSeconds(normalDuration);
                
            current++;
        }
        
        InputManager.Instance.canTakeInput = true;
    }

    IEnumerator JumpTravel(List<Grid> path)
    {
        InputManager.Instance.canTakeInput = false;
        
        int current = 0;
        while (current + 1 < path.Count)
        {
            Vector3 targetDirection = (path[current+1].transform.position - path[current].transform.position).normalized;

            Vector3 currentDirection = transform.forward;

            float angle = Vector3.SignedAngle(currentDirection, targetDirection, Vector3.up);

            if (Mathf.Abs(angle)>0.01)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

                transform.DORotate(targetRotation.eulerAngles, jumpDuration, RotateMode.Fast);

                yield return new WaitForSeconds(jumpDuration);
            }
            var pos = new Vector3(path[current+1].transform.position.x,transform.position.y,path[current+1].transform.position.z);
            transform.DOJump(pos, jumpPower, jumpNumber, jumpDuration);

            yield return new WaitForSeconds(jumpDuration);
                
            current++;
        }
        
        InputManager.Instance.canTakeInput = true;

    }

    public IEnumerator Turn(Vector3 yourPosition, Vector3 targetPosition, Action action)
    {
        Vector3 targetDirection = (targetPosition - yourPosition).normalized;

        Vector3 currentDirection = transform.forward;

        float angle = Vector3.SignedAngle(currentDirection, targetDirection, Vector3.up);

        if (Mathf.Abs(angle)>0.01)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            transform.DORotate(targetRotation.eulerAngles, jumpDuration, RotateMode.Fast);

            yield return new WaitForSeconds(jumpDuration);
        }
        
        action?.Invoke();
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
            
            tempList.Add(input_path[input_path.Count-1]);
            
        }
        return listHolder;
    }
}
