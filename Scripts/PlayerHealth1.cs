using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Vida")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    [Header("Sonido")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip damageSound;

    public bool IsDead { get; private set; }

    // Evento para que la UI (barra de vida) se actualice sin acoplarse directamente
    public event Action<float, float> OnHealthChanged; // (current, max)
    public event Action OnPlayerDied;

    private void Awake()
    {
        currentHealth = maxHealth;
        IsDead = false;
    }

    private void Start()
    {
        // Avisamos el valor inicial para que la barra de vida arranque llena
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(float amount)
    {
        if (IsDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (audioSource != null && damageSound != null)
        {
            audioSource.PlayOneShot(damageSound);
        }

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (IsDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void FullHeal()
    {
        if (IsDead) return;

        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Die()
    {
        IsDead = true;
        OnPlayerDied?.Invoke();
        Debug.Log("El jugador murió");

        // Aca despues podemos enganchar pantalla de Game Over, desactivar controles, etc.
    }
}