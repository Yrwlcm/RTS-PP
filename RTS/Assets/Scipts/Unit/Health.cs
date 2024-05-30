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
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        healthBar.SetColor(Teams.Colors[unit.Team]);
    }

    private void Update()
    {
        healthBar.SetHealth(currentHealth, maxHealth);
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth  + amount, 0, maxHealth);
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
        animator.SetBool("Death", true);
        Destroy(gameObject, 2f);
    }
}