using UnityEngine;

public class CameraHeadBob : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private CharacterController playerController;

    [Header("Bobbing")]
    [SerializeField] private float bobFrequency = 8f;   // que tan rapido sube y baja
    [SerializeField] private float bobAmplitude = 0.05f; // que tanto sube y baja
    [SerializeField] private float smoothSpeed = 8f;      // que tan fluido es el movimiento

    private Vector3 initialLocalPos;
    private float bobTimer;

    private void Awake()
    {
        initialLocalPos = transform.localPosition;
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        bool isMoving = (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
                         && playerController != null && playerController.isGrounded;

        Vector3 targetPos = initialLocalPos;

        if (isMoving)
        {
            bobTimer += Time.deltaTime * bobFrequency;
            float bobOffset = Mathf.Sin(bobTimer) * bobAmplitude;
            targetPos += new Vector3(0f, bobOffset, 0f);
        }
        else
        {
            // Vuelve a fase 0 para que el proximo paso arranque suave, no de cualquier punto
            bobTimer = 0f;
        }

        // Interpolamos siempre hacia el target, para que sea fluido y no tosco
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, smoothSpeed * Time.deltaTime);
    }
}