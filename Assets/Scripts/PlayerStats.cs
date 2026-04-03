using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;

    [SerializeField] private float currentStamina;
    [SerializeField] private float maxStamina;

    void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
    }

    public void TakeDamge(float damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            
        }
    }
}
