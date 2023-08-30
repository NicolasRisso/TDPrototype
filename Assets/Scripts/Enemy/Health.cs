using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health Status")]
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] private HealthBar healthBar;

    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        if (healthBar == null) Debug.Log("AAAAAAAAAAAAA");
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
        Destroy(gameObject);
    }
}
