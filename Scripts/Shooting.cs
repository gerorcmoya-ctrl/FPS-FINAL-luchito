using UnityEngine;

public class Shooting : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform muzzlePoint; // punta del arma, de donde sale la bala
    [SerializeField] private GameObject bulletPrefab; // tu modelo de bala
    [SerializeField] private WeaponRecoil weaponRecoil; // script en el objeto del arma
    [SerializeField] private MuzzleFlash muzzleFlash;   // script en el objeto de la particula

    [Header("Disparo")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 100f;

    [Header("Sonido")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootSound;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1")) // click izquierdo
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        // Efecto visual: instanciamos la bala en la punta del arma
        if (bulletPrefab != null && muzzlePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, muzzlePoint.position, playerCamera.transform.rotation);
            if (bullet.GetComponent<BulletVisual>() == null)
            {
                bullet.AddComponent<BulletVisual>();
            }
        }

        // Recoil del arma
        if (weaponRecoil != null)
        {
            weaponRecoil.DoRecoil();
        }

        // Flash de disparo
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        // Sonido de disparo
        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        // Dano real: raycast instantaneo desde el centro de la camara
        RaycastHit hit;
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out hit, range))
        {
            Debug.Log("Le pegue a: " + hit.transform.name);

            IDamageable damageable = hit.transform.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
        }
    }
}