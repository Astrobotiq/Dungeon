using System;
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

    public IEnumerator Template()
    {
        var attackPos = Dedice();
        TargetGrid = GridManager.Instance.getGridFromLocation(attackPos);
        Move(attackPos);
        while (transform.position.x != TargetGrid.transform.position.x && transform.position.z != TargetGrid.transform.position.z)
        {
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        DecideAttackTile();
        PreAttack();
    }

    protected abstract void DecideAttackTile();

    public abstract Vector3 Dedice();

    public void Move(Vector3 attackPos)
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
            EnemyManager.Instance.SelectEnemy(this);
            AttackBTN.SetActive(isActivate);
        }
    }
}
