using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EnemySpawnEntry
{
    public GameObject enemyPrefab;
    public int count = 3;
    public bool isBoss = false; // tildar en la entrada que sea el boss, para conectar su barra de vida
}

[System.Serializable]
public class RoundData
{
    public string roundName = "Ronda";
    public EnemySpawnEntry[] enemies;
}

public class GameManager : MonoBehaviour
{
    [Header("Puntos de spawn")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Configuracion de las 3 rondas")]
    [SerializeField] private RoundData[] rounds;

    [Header("Tiempo entre rondas")]
    [SerializeField] private float delayBetweenRounds = 3f;

    [Header("Evitar superposicion")]
    [SerializeField] private float spawnScatterRadius = 1.5f; // separacion al azar entre enemigos del mismo punto

    [Header("Power-up de vida")]
    [SerializeField] private GameObject healthPickupPrefab;
    [SerializeField] private Transform player; // si se deja vacio, se busca solo

    [Header("Barra de vida del boss")]
    [SerializeField] private BossHealthUI bossHealthUI;

    [Header("Contador de kills")]
    [SerializeField] private TMPro.TextMeshProUGUI killCounterText;

    [Header("Pantalla de victoria")]
    [SerializeField] private VictoryUI victoryUI;

    private int currentRoundIndex = -1;
    private int enemiesAliveInRound = 0;
    private int totalEnemiesInRound = 0;
    private int killsInRound = 0;

    private void Start()
    {
        if (player == null)
        {
            PlayerHealth foundPlayerHealth = FindFirstObjectByType<PlayerHealth>();
            if (foundPlayerHealth != null)
            {
                player = foundPlayerHealth.transform;
            }
        }

        StartNextRound();
    }

    private void StartNextRound()
    {
        currentRoundIndex++;

        if (currentRoundIndex >= rounds.Length)
        {
            Debug.Log("Ganaste! Todas las rondas completadas.");

            if (victoryUI != null)
            {
                victoryUI.Show();
            }

            return;
        }

        RoundData round = rounds[currentRoundIndex];
        Debug.Log("Empieza: " + round.roundName);

        SpawnRound(round);
    }

    private void SpawnRound(RoundData round)
    {
        enemiesAliveInRound = 0;
        killsInRound = 0;

        totalEnemiesInRound = 0;
        foreach (EnemySpawnEntry entry in round.enemies)
        {
            totalEnemiesInRound += entry.count;
        }

        UpdateKillCounterUI();

        foreach (EnemySpawnEntry entry in round.enemies)
        {
            for (int i = 0; i < entry.count; i++)
            {
                SpawnEnemy(entry);
            }
        }
    }

    private void SpawnEnemy(EnemySpawnEntry entry)
    {
        if (entry.enemyPrefab == null || spawnPoints.Length == 0) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Pequeno desplazamiento al azar para que no queden superpuestos si cae el mismo punto
        Vector2 randomOffset = Random.insideUnitCircle * spawnScatterRadius;
        Vector3 spawnPosition = spawnPoint.position + new Vector3(randomOffset.x, 0f, randomOffset.y);

        GameObject enemyInstance = Instantiate(entry.enemyPrefab, spawnPosition, spawnPoint.rotation);

        EnemyHealth health = enemyInstance.GetComponent<EnemyHealth>();
        if (health != null)
        {
            enemiesAliveInRound++;
            health.OnEnemyDied += HandleEnemyDied;

            if (entry.isBoss && bossHealthUI != null)
            {
                bossHealthUI.Show(health);
            }
        }
    }

    private void HandleEnemyDied(EnemyHealth enemy)
    {
        enemy.OnEnemyDied -= HandleEnemyDied;
        enemiesAliveInRound--;
        killsInRound++;

        UpdateKillCounterUI();

        if (enemiesAliveInRound <= 0)
        {
            SpawnHealthPickup();
            StartCoroutine(NextRoundAfterDelay());
        }
    }

    private void UpdateKillCounterUI()
    {
        if (killCounterText != null)
        {
            killCounterText.text = "KILLS: " + killsInRound + "/" + totalEnemiesInRound;
        }
    }

    private void SpawnHealthPickup()
    {
        if (healthPickupPrefab == null || player == null) return;

        // Aparece un poco delante del jugador, a la altura del piso donde esta parado
        Vector3 spawnPosition = player.position + player.forward * 2f;
        Instantiate(healthPickupPrefab, spawnPosition, Quaternion.identity);
    }

    private IEnumerator NextRoundAfterDelay()
    {
        yield return new WaitForSeconds(delayBetweenRounds);
        StartNextRound();
    }
}