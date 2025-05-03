using System;
using UnityEngine;

public class IHealth : MonoBehaviour
{
    [SerializeField]
    protected int currentHealth;

    [SerializeField] 
    protected int maxHealth;
    
    [SerializeField]
    protected float damageMultiplier;
    
    public event Action OnDeath;
    public event Action OnHeal;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage, bool willPush = false)
    {
        currentHealth -= damage;
        
        if (currentHealth<=0)
        {
            if (gameObject.tag.Equals("Enemy"))
            {
                if (TryGetComponent<LineController>(out var lineController))
                {
                    lineController.RemoveLine();
                }
                
                var enemyBrain = GetComponent<EnemyBrain>();
                
                if (willPush)
                {
                    var pushTime = GetComponent<IPushable>().GetDuration();
                    enemyBrain.OnDeath(pushTime+ 1f);
                }
                else
                {
                    enemyBrain.OnDeath();
                }
            }
            else
            {
                GridManager.Instance.getGridFromLocation(transform.position).GridObject = null;
                Destroy(this.gameObject);
            }
            
        }
        
        //InGameUITextMesh.Instance.UpdatePlayerBars();
    }

    

    public virtual void Heal(int heal)
    {
        currentHealth += heal;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public float getHealthPercentage()
    {
        float temp_1 = (float)currentHealth;
        float temp_2 = (float)maxHealth;

        float temp_3 = Mathf.FloorToInt((temp_1 / temp_2) * 100);
        
        //Debug.Log("health percentage " + temp_3);
        return temp_3; 

    }

    public int getHealth()
    {
        return currentHealth;
    }
}