using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public abstract class EnemyBrain : MonoBehaviour
{
    public Sprite enemyPortrait;
    
    public MaterialController MaterialController;
    public Renderer Renderer;
    
    [SerializeField]
    protected Move move;
    
    [SerializeField]
    protected Grid currentGrid;
    
    [SerializeField]
    protected Grid TargetGrid;

    [SerializeField] 
    protected GameObject AttackBTN;
    
    [SerializeField] 
    protected GameObject AttackBTN2;

    protected bool _hasFinishedMoving;

    public int InitiationPoint;
    
    [SerializeField]
    protected int InitiationBonus = 0;
    
    [SerializeField]
    protected float liftHeight = 1.5f; // Havaya kalkma yüksekliği
    
    [SerializeField]
    protected float rotationAngle = 90f; // Yana dönme açısı
    
    [SerializeField]
    protected float duration = 1.5f; // Hareket süresi
    

    public void SetFinishMove(bool hasFinished)
    {
        _hasFinishedMoving = hasFinished;
    }
    
    
    //In one turn enemy do this action one by one
    //Decide where to go (Can)
    //Go (Ready)
    //Which side to attack (will be ready)
    //Pre Attack (will be ready)
    //Attack (will be ready)

    void Start()
    {
        EnemyManager.Subscribe(this);

        currentGrid = GridManager.Instance.getGridFromLocation(transform.position);

        currentGrid.GridObject = gameObject;

        _hasFinishedMoving = false;

        InitiationPoint = Random.Range(1, 10) + InitiationBonus;
        
        AttackBTN.GetComponent<Button>().onClick.AddListener((() =>
        {
            StartCoroutine(Template());
        }));
        
        AttackBTN2.GetComponent<Button>().onClick.AddListener((() =>
        {
            OnDeath();
        }));
        
        MaterialController = new MaterialController(Renderer, 0.5f);
    }

    public void UIRefHoverEnter()
    {
        MaterialController.SetOutlineScale();
    }

    public void UIRefHoverExit()
    {
        MaterialController.ResetOutlineScale();
    }

    void OnDestroy()
    {
        EnemyManager.Unsubscribe(this);
    }

    public void SetGrid(Grid grid) => currentGrid = grid;

    public Grid GetTargetGrid() =>  TargetGrid;

    public void SetTargetGrid(Grid newTarget)
    {
        TargetGrid = newTarget;
        RecalculateTarget("");
    }

    public IEnumerator Template()
    {
        _hasFinishedMoving = false;
        var attackPos = Dedice();
        TargetGrid = GridManager.Instance.getGridFromLocation(attackPos);
        Move(attackPos);
        while (!_hasFinishedMoving)
        {
            yield return null;
        }
        Debug.Log("Template 1");
        yield return new WaitForSeconds(2f);
        Debug.Log("Template 2");
        DecideAttackTile();
        PreAttack();
    }

    public virtual void RecalculateTarget(string name){}

    protected abstract void DecideAttackTile();

    public abstract Vector3 Dedice();

    public virtual void Move(Vector3 attackPos)
    {
        move.StartMove(currentGrid,GridManager.Instance.getGridFromLocation(attackPos));
    }

    public virtual void PreAttack(){}

    public abstract void Attack();

    public void OnEnemySelection(bool isActivate)
    {
        Debug.Log($"Enemy seçilmiş : {isActivate}");
        if (AttackBTN != null)
        {
            if (isActivate)
                EnemyManager.Instance.SelectEnemy(this);
            AttackBTN.SetActive(isActivate);
            AttackBTN2.SetActive(isActivate);
        }
    }

    public void OnDeath()
    {
        AnimateDeath((() =>
        {
            FeelManager.Instance.ShakeCamera();
            ArmController.Instance.RemoveEnemyFromTable(transform.position,0.8f);
        }));
    }

    private void AnimateDeath(Action action = null)
    {
        Vector3 originalPosition = transform.position;
        Vector3 liftedPosition = originalPosition + Vector3.up * liftHeight;

        Sequence sequence = DOTween.Sequence();

        // 1. Yukarı çıkarken geriye doğru eğilme
        sequence.Append(transform.DOMove(liftedPosition, duration / 2).SetEase(Ease.OutQuad));
        sequence.Join(transform.DORotate(new Vector3(rotationAngle / 2, 0, 0), duration / 2));

        // 2. Aşağı inerken tamamen sırt üstü düşme
        sequence.Append(transform.DOMove(originalPosition, duration / 2).SetEase(Ease.InQuad));
        sequence.Join(transform.DORotate(new Vector3(rotationAngle, 0, 0), duration / 2).OnComplete((() => action.Invoke())));
;
    }
    
}
