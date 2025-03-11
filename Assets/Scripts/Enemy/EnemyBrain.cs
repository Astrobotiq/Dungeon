using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class EnemyBrain : MonoBehaviour
{
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
    private int InitiationBonus = 0;

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
            Attack();
        }));
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
}
