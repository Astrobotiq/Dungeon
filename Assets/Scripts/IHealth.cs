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
}