using UnityEngine;

public class BulletVisual : MonoBehaviour
{
    [SerializeField] private float speed = 60f;
    [SerializeField] private float lifeTime = 2f;

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}