using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class SpiderEnemyBrain : EnemyBrain
{
    [SerializeField]
    private GameObject web;
    
    [SerializeField]
    private GameObject attackEffect;
    
    [SerializeField]
    private float attackStartTime = 0.5f;
    
    [SerializeField]
    private float attackDashTime = 0.15f;
    
    [SerializeField]
    private float attackRecoveryTime = 0.4f;
    

    public override Vector3 Dedice()
    {
        /*List<List<GameObject>> GridList = GridManager.Instance.GridList;

        foreach (var list in GridList)
        {
            foreach (var gridObj in list)
            {
                if (gridObj.GetComponent<Grid>().GridObject && gridObj.GetComponent<Grid>().GridObject.tag.Equals("Player"))
                {
                    return new Vector3(gridObj.transform.position.x-1, gridObj.transform.position.y, gridObj.transform.position.z);
                }
            }
        }

        return currentGrid.transform.position;*/
        
        
        Vector3 bestOption = gameObject.GetComponent<EnemyAI_Organizer>().ReturnBestOption(gameObject);
        Debug.Log("seçtiğim en iyi loc " + bestOption);
        return bestOption;
    }
    
    protected override void DecideAttackTile()
    {
        currentGrid = GridManager.Instance.getGridFromLocation(transform.position);
        var currentGridPos = currentGrid.transform.position;

        List<Grid> targetGrids = new List<Grid>();

        for (int i = -1; i <= 1; i++)
        {
            if (i==0)
            {
                continue;
            }
            
            var xGrid = GridManager.Instance.getGridFromLocation(new Vector3(currentGridPos.x + i, currentGridPos.y, currentGridPos.z));
            var zGrid = GridManager.Instance.getGridFromLocation(new Vector3(currentGridPos.x, currentGridPos.y, currentGridPos.z + i));

            if (xGrid.gameObject && xGrid.GridObject && xGrid.GridObject.tag.Equals("Player"))
            {
                targetGrids.Add(xGrid);
            }
            
            if (zGrid.gameObject && zGrid.GridObject && zGrid.GridObject.tag.Equals("Player"))
            {
                targetGrids.Add(zGrid);
            }
        }

        if (targetGrids.Count == 0)
        {
            Debug.LogError("Target grid listesi boş. Koduna bak");
            return;
        }

        TargetGrid = targetGrids.GetRandom();
    }

    public override void PreAttack()
    {
        if (!TargetGrid)
        {
            Debug.LogError("Spider ağ atacak target bulamadı.");
            return;
        }

        StartCoroutine(move.Turn(transform.position, TargetGrid.GridObject.transform.position, null));
        
        Instantiate(web,
            new Vector3(TargetGrid.transform.position.x, TargetGrid.transform.position.y + 0.5f,
                TargetGrid.transform.position.z), Quaternion.identity);

    }

    public override void Attack()
    {
        var effectStartPos = transform.position;

        var diffrence = TargetGrid.transform.position - transform.position;
        diffrence = new Vector3(diffrence.x, 0, diffrence.z);
        
        var effect =Instantiate(attackEffect, effectStartPos, quaternion.identity);

        StartCoroutine(DestroyEffect(effect));

        transform.DOMove(transform.position - (diffrence / 4), attackStartTime).OnComplete((() =>
        {
            transform.DOMove(transform.position + (diffrence / 2), attackDashTime).OnComplete((() =>
            {
                FeelManager.Instance.ShakeCamera();
                TargetGrid.GridObject.GetComponent<IHealth>().TakeDamage(5);
                transform.DOMove(effectStartPos, attackRecoveryTime);
            }));
        }));

        IEnumerator DestroyEffect(GameObject effect)
        {
            yield return new WaitForSeconds(2f);
            Destroy(effect);
        }

    }
}
