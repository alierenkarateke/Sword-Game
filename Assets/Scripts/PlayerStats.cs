using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;

    [SerializeField] private float currentStamina;
    [SerializeField] private float maxStamina;

    [SerializeField] public string eTag;

    void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
    }

    public void TakeDamge(float damage, string tag)
    {
        currentHealth -= damage;
        Debug.Log(tag + " current health = " + currentHealth);
        if(currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
