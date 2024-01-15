using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health Status")]
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private GameObject deathSound;

    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void Damage(float damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0f) Kill();
    }

    private void Kill()
    {
        Instantiate(deathSound, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
