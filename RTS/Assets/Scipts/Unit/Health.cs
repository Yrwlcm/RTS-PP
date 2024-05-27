using Scipts;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    [SerializeField] private FloatingHealthBar healthBar;
    [SerializeField] private Unit unit;

    public float CurrentHealth => currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        healthBar.SetColor(Teams.Colors[unit.Team]);
    }

    private void Update()
    {
        healthBar.SetHealth(currentHealth, maxHealth);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}