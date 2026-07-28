using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyAI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform player;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Animator animator; // opcional, para el trigger de Attack (usado por el boss)

    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 3f;

    [Header("Ataque")]
    [SerializeField] private float attackRange = 2.5f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 1.5f;

    private Rigidbody rb;
    private float lastAttackTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

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

        if (distanceToPlayer > attackRange)
        {
            ChasePlayer();
        }
        else
        {
            // Frena en seco al llegar a rango de ataque, no sigue empujando al player
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);

            if (Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
            }
        }
    }

    private void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position);
        direction.y = 0f;
        direction.Normalize();

        // Movemos por velocidad, no por MovePosition, para que la fisica resuelva bien las colisiones
        Vector3 desiredVelocity = direction * moveSpeed;
        rb.linearVelocity = new Vector3(desiredVelocity.x, rb.linearVelocity.y, desiredVelocity.z);

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(targetRotation);
        }
    }

    private void Attack()
    {
        lastAttackTime = Time.time;

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }
}