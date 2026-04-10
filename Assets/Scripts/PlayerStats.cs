using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;

    [SerializeField] private float currentStamina;
    [SerializeField] private float maxStamina;

    [SerializeField] public string eTag;
    [SerializeField] public float hitFlashDuration;

    [SerializeField] public Color hitFlashColor;
    [SerializeField] Renderer[] meshRenderer;

    Color[] originalColors;

    void Start()
    {
        currentHealth = maxHealth;
    currentStamina = maxStamina;

    // tüm materialleri say
    int totalMaterials = 0;
    foreach(Renderer r in meshRenderer)
        totalMaterials += r.materials.Length;

    originalColors = new Color[totalMaterials];

    int index = 0;
    foreach(Renderer r in meshRenderer)
        foreach(Material mat in r.materials)
            originalColors[index++] = mat.color;
    }

    public void TakeDamge(float damage, string tag)
    {
        StartCoroutine(hitFlash());
        currentHealth -= damage;
        Debug.Log(tag + " current health = " + currentHealth);
        if(currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

     IEnumerator hitFlash()
        {
            foreach(Renderer r in meshRenderer)
        foreach(Material mat in r.materials)
            mat.color = hitFlashColor;

    yield return new WaitForSeconds(hitFlashDuration);

    int index = 0;
    foreach(Renderer r in meshRenderer)
        foreach(Material mat in r.materials)
            mat.color = originalColors[index++];
        }
}
