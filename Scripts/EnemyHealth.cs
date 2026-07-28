using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Header("Vida")]
    [SerializeField] private float maxHealth = 50f;
    private float currentHealth;

    [Header("Efecto al morir")]
    [SerializeField] private bool explodeOnDeath = true; // destildar en el boss
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private AudioClip deathSound;

    public bool IsDead { get; private set; }

    // Otros scripts (barra de vida, GameManager para contar kills) se suscriben a esto
    public event Action<float, float> OnHealthChanged; // (current, max)
    public event Action<EnemyHealth> OnEnemyDied;

    private void Awake()
    {
        currentHealth = maxHealth;
        IsDead = false;
    }

    private void Start()
    {
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(float amount)
    {
        if (IsDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        IsDead = true;
        OnEnemyDied?.Invoke(this);
        Debug.Log(gameObject.name + " murio");

        if (explodeOnDeath && explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        // Usamos PlayClipAtPoint porque el objeto se destruye enseguida:
        // esto crea un audio temporal en ese punto que se reproduce solo y se autodestruye
        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
        }

        // Destruimos el enemigo. Si despues queres agregar animacion de muerte,
        // aca conviene esperar un tiempo antes de destruir (con una corutina).
        Destroy(gameObject, 0.1f);
    }
}