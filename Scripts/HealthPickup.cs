using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [Header("Efecto visual")]
    [SerializeField] private float rotateSpeed = 90f; // grados por segundo, gira solo para llamar la atencion

    private void Update()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.FullHeal();
            Destroy(gameObject);
        }
    }
}