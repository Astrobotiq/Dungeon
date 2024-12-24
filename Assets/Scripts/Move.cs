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
        /*int current = 0;
        while (current<path.Count)
        {
            var startGrid = path[current];

            var nextGrid = path[++current];

            var diff = nextGrid.transform.position -startGrid.transform.position;

            bool isOnX = diff.x != 0;
            bool isOnZ = diff.z != 0;

            if (isOnX && isOnZ)
            {
                Debug.Log("Çapraza yol bulundu");
                yield break;
            }

            while (path[])
            {
                
            }
        }*/
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
}
