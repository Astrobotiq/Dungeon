using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using Sequence = DG.Tweening.Sequence;

public class RangerEnemyBrain : EnemyBrain
{
    [SerializeField]
    private GameObject attackEffect;
    
    [SerializeField]
    private float attackDashTime = 0.15f;
    
    [SerializeField]
    private float attackRecoveryTime = 0.4f;
    
    private LineController _targetLineController;

    void Awake()
    {
        _targetLineController = GetComponent<LineController>();
        
    }

    void OnEnable()
    {
        EventManager.onMove += RecalculateTarget;
        EventManager.onPush += RecalculateTarget;
    }

    void OnDisable()
    {
        EventManager.onMove -= RecalculateTarget;
        EventManager.onPush -= RecalculateTarget;
    }

    public void RecalculateTarget()
    {
        RecalculateTarget("");
    }

    public override void RecalculateTarget(string name)
    {
        if (name.Equals(gameObject.name))
        {
            return;
        }
        
        if (TargetGrid == null)
            return;
        
        Vector3 difference = TargetGrid.transform.position - transform.position;
        difference = new Vector3(difference.x, 0, difference.z);
        Vector3 currentPos = transform.position;


        Vector2Int direction;
        if (Mathf.Abs(difference.x) > Mathf.Abs(difference.z))
        {
            direction = difference.x > 0 ? Vector2Int.right : Vector2Int.left;
        }
        else
        {
            direction = difference.z > 0 ? Vector2Int.up : Vector2Int.down;
        }
        
        for (int i = 0; i < 10; i++)
        {
            Vector3 nextPos = new Vector3(currentPos.x + direction.x,currentPos.y,currentPos.z + direction.y);
            
                
            Grid grid = GridManager.Instance.getGridFromLocation(nextPos);
            if (grid == null)
            {
                nextPos = new Vector3(nextPos.x - direction.x,nextPos.y,nextPos.z - direction.y);
                grid = GridManager.Instance.getGridFromLocation(nextPos);
                TargetGrid = grid;
                _targetLineController.DrawLine(transform.position,nextPos);
                break;
            }
            

            if (grid.GridObject != null && grid.GridObject.GetComponent<Water>() == null)
            {
                TargetGrid = grid;
                _targetLineController.DrawLine(transform.position,TargetGrid.GridObject.transform.position);
                return;
            }

            currentPos = nextPos;
        }
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
            
            if (grid.GridObject.TryGetComponent<EnemyBrain>(out var enemy) || grid.GridObject.gameObject.CompareTag("Water"))
            {
                continue;
            }
            
            possibleAttackPosition.Add(pos);
        }

        if (possibleAttackPosition.Count == 0)
        {
            Debug.Log("Saldırılacak player bulunamadı");
            TargetGrid = null;
            return;
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
        if (!TargetGrid)
        {
            return;
        }
        
        StartCoroutine(move.Turn(transform.position, TargetGrid.GridObject.transform.position, (
            () => _targetLineController.DrawLine(transform.position, TargetGrid.GridObject.transform.position))));
    }

    public override void Attack()
    {
        if (!TargetGrid || isDead)
        {
            return;
        }
        
        var effectStartPos = transform.position;

        var diffrence = TargetGrid.transform.position - transform.position;
        var diff = new Vector3(diffrence.x, 0, diffrence.z);
        diffrence = new Vector3(diffrence.x, 0, diffrence.z).normalized;
        
        
        var effect =Instantiate(attackEffect, effectStartPos, Quaternion.LookRotation(diffrence));
        effect.GetComponent<ISkillEffect>().StartMoving(TargetGrid);
        
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(transform.position - (diff / 5), attackDashTime));
        sequence.Append(transform.DOMove(effectStartPos, attackRecoveryTime));
        sequence.Play();
        _targetLineController.RemoveLine();
        TargetGrid = null;
    }
    
    
}