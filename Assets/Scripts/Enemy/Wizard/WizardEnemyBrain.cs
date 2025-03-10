using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class WizardEnemyBrain : EnemyBrain
{
    
    [SerializeField]
    private GameObject attackEffect;
    
    [SerializeField]
    private float attackAnimHeight = 0.5f;
    
    [SerializeField]
    private AnimationCurve attackFlyCurve;
    
    [SerializeField]
    private float attackDashTime = 0.15f;
    
    [SerializeField]
    private float attackRecoveryTime = 0.4f;
    
    private LineController _targetLineController;

    void Awake()
    {
        _targetLineController = GetComponent<LineController>();
    }

    protected override void DecideAttackTile()
    {
        AlgorithmSkillFourDirection algorithm = new AlgorithmSkillFourDirection();
        
        currentGrid = GridManager.Instance.getGridFromLocation(transform.position);

        var hashSet = algorithm.startAlgorithm(currentGrid, 7, false);
        List<Vector3> possibleAttackPosition = new List<Vector3>();

        foreach (var pos in hashSet)
        {
            var grid = GridManager.Instance.getGridFromLocation(pos);
            
            if (grid.GridObject == null)
            {
                continue;
            }
            
            possibleAttackPosition.Add(pos);
        }

        if (possibleAttackPosition.Count == 0)
        {
            Debug.Log("Saldırılacak player bulunamadı");
        }
        Debug.Log($"possible count : {possibleAttackPosition.Count}");
        
        TargetGrid = GridManager.Instance.getGridFromLocation(possibleAttackPosition.GetRandom());
    }

    public override Vector3 Dedice()
    {
        /*List<List<GameObject>> GridList = GridManager.Instance.GridList;

        foreach (var list in GridList)
        {
            foreach (var gridObj in list)
            {
                if (gridObj.GetComponent<Grid>().GridObject && gridObj.GetComponent<Grid>().GridObject.tag.Equals("Player"))
                {
                    var place = Random.Range(2, 3);
                    return Random.Range(0f,1f)>0.5f ?  
                        new Vector3(gridObj.transform.position.x - place, gridObj.transform.position.y, gridObj.transform.position.z):  
                        new Vector3(gridObj.transform.position.x , gridObj.transform.position.y, gridObj.transform.position.z - place);
                }
            }
        }

        return currentGrid.transform.position;*/
        
        Vector3 bestOption = gameObject.GetComponent<EnemyAI_Organizer>().ReturnBestOption(gameObject);
        Debug.Log("seçtiğim en iyi loc " + bestOption);
        return bestOption;
    }

    public override void PreAttack()
    {
        StartCoroutine(move.Turn(transform.position, TargetGrid.GridObject.transform.position, (
            () => _targetLineController.DrawLine(transform.position,TargetGrid.GridObject.transform.position) 
                )));
        
    }

    public override void Attack()
    {
        var effectStartPos = transform.position;

        var diffrence = new Vector3(0, attackAnimHeight, 0);
        
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(transform.position + diffrence, attackDashTime).SetEase(attackFlyCurve));
        sequence.Append(transform.DOMove(effectStartPos, attackRecoveryTime).OnComplete((() =>
        {
            FeelManager.Instance.ShakeCamera();
            var effect =Instantiate(attackEffect, TargetGrid.transform.position + diffrence, Quaternion.identity);
            if (effect)
            {
                effect.GetComponent<ISkillEffect>().StartMoving(TargetGrid);
            }
        })));
        sequence.Play();
        _targetLineController.RemoveLine();
    }
}
