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

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("ben geldim naber");
        InGameUITextMesh.Instance.UpdatePlayerBars();

        if (currentHealth<=0)
        {
            if (gameObject.tag.Equals("Enemy"))
            {
                var enemyBrain = GetComponent<EnemyBrain>();
                enemyBrain.OnDeath();
            }
            else
            {
                GridManager.Instance.getGridFromLocation(transform.position).GridObject = null;
                Destroy(this.gameObject);
            }
            
        }
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
        
        Debug.Log("health percentage " + temp_3);
        return temp_3; 

    }

    public int getHealth()
    {
        return currentHealth;
    }
}