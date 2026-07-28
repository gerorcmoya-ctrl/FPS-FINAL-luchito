using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject container; // el objeto que agrupa la barra entera
    [SerializeField] private Image healthFillImage;

    private EnemyHealth bossHealth;

    private void Awake()
    {
        // Arranca oculta, solo se muestra cuando aparece el boss
        if (container != null)
        {
            container.SetActive(false);
        }
    }

    public void Show(EnemyHealth boss)
    {
        bossHealth = boss;
        bossHealth.OnHealthChanged += HandleHealthChanged;
        bossHealth.OnEnemyDied += HandleBossDied;

        if (container != null)
        {
            container.SetActive(true);
        }
    }

    private void HandleHealthChanged(float current, float max)
    {
        if (healthFillImage != null)
        {
            healthFillImage.fillAmount = current / max;
        }
    }

    private void HandleBossDied(EnemyHealth enemy)
    {
        bossHealth.OnHealthChanged -= HandleHealthChanged;
        bossHealth.OnEnemyDied -= HandleBossDied;

        if (container != null)
        {
            container.SetActive(false);
        }
    }
}