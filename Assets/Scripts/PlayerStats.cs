using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    #region Settings
    [Header("Position")]
    [SerializeField] private Vector3 startPosition;

    [Header("Health")]
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;
    [SerializeField] private Slider healthBar;

    [Header("Hit Flash")]
    [SerializeField] private float hitFlashDuration;
    [SerializeField] private Color hitFlashColor;
    [SerializeField] Renderer[] meshRenderer;


    #endregion Settings
     
    #region UnityLifecycle

    void Start()
    {
        startPosition = transform.position;
        currentHealth = maxHealth;
        healthBar.value = 1f;
    }
    
    #endregion UnityLifecycle

    #region Methods

    IEnumerator hitFlash()
    {
        SetEmisson(hitFlashColor);
        yield return new WaitForSeconds(hitFlashDuration);
        SetEmisson(Color.black);
    }

    private void SetEmisson(Color color)
    {
        foreach (Renderer r in meshRenderer)
            foreach (Material mat in r.materials)
                if (mat.HasProperty("_EmissionColor"))
                {
                    mat.EnableKeyword("_EMISSION");
                    mat.SetColor("_EmissionColor", color);
                }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth / maxHealth;

        StartCoroutine(hitFlash());
        HealthControl();
    }

    private void HealthControl()
    {
        if (currentHealth <= 0)
        {
            GetComponentInChildren<Animator>().SetBool("isDead", true);
            GameManager.Instance.PlayerDied(gameObject.tag);
        }
    }

    public void ResetPlayer()
    {
     currentHealth = maxHealth;
     healthBar.value = 1f;
     transform.position = startPosition;   
    }

    #endregion Methods
}
