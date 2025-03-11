using UnityEngine;

public class IHealth : MonoBehaviour
{
    [SerializeField]
    protected int currentHealth;

    [SerializeField] 
    protected int maxHealth;
    
    [SerializeField]
    protected float damageMultiplier;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }

    public virtual void Heal(int heal)
    {
        currentHealth += heal;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public int getHealth() {
        return currentHealth;
    }

    public float getHealthPercentage()
    {
        float temp_1 = (float)(currentHealth);
        float temp_2 = (float)(maxHealth);
        return  temp_1/temp_2  * 100;
    }
}