using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RangedEnemyAI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform player;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Animator animator;

    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 3f;

    [Header("Ataque a distancia")]
    [SerializeField] private float attackRange = 8f; // distancia a la que se detiene y dispara
    [SerializeField] private float shootDamage = 8f;
    [SerializeField] private float shootCooldown = 2f;

    [Header("Bala visual")]
    [SerializeField] private Transform muzzlePoint; // de donde sale la bala
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 25f; // mas bajo que la del jugador (60)

    private Rigidbody rb;
    private float lastShootTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        // Al venir de un Prefab, las referencias a objetos de la escena (el Player)
        // se pierden. Las buscamos solos si no estan asignadas.
        if (player == null || playerHealth == null)
        {
            PlayerHealth foundPlayerHealth = FindFirstObjectByType<PlayerHealth>();
            if (foundPlayerHealth != null)
            {
                playerHealth = foundPlayerHealth;
                player = foundPlayerHealth.transform;
            }
        }
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Siempre mira hacia el jugador
        Vector3 lookDirection = (player.position - transform.position);
        lookDirection.y = 0f;
        if (lookDirection != Vector3.zero)
        {
            rb.MoveRotation(Quaternion.LookRotation(lookDirection));
        }

        if (distanceToPlayer > attackRange)
        {
            // Se acerca hasta quedar en rango
            Vector3 direction = lookDirection.normalized;
            Vector3 desiredVelocity = direction * moveSpeed;
            rb.linearVelocity = new Vector3(desiredVelocity.x, rb.linearVelocity.y, desiredVelocity.z);

            SetMoving(true);
        }
        else
        {
            // Esta en rango: se detiene y dispara
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            SetMoving(false);

            if (Time.time >= lastShootTime + shootCooldown)
            {
                Shoot();
            }
        }
    }

    private void SetMoving(bool isMoving)
    {
        if (animator != null)
        {
            animator.SetBool("IsMoving", isMoving);
        }
    }

    private void Shoot()
    {
        lastShootTime = Time.time;

        if (animator != null)
        {
            animator.SetTrigger("Shoot");
        }

        // Bala visual: sale del muzzlePoint apuntando hacia el jugador
        if (bulletPrefab != null && muzzlePoint != null && player != null)
        {
            Vector3 directionToPlayer = (player.position - muzzlePoint.position).normalized;
            Quaternion bulletRotation = Quaternion.LookRotation(directionToPlayer);

            GameObject bullet = Instantiate(bulletPrefab, muzzlePoint.position, bulletRotation);
            BulletVisual bulletVisual = bullet.GetComponent<BulletVisual>();
            if (bulletVisual == null)
            {
                bulletVisual = bullet.AddComponent<BulletVisual>();
            }
            bulletVisual.SetSpeed(bulletSpeed);
        }

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(shootDamage);
        }
    }
}