using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("Sensibilidad")]
    [SerializeField] private float mouseSensitivity = 200f;

    [Header("Referencias")]
    [SerializeField] private Transform playerBody; // el objeto Player (que rota en Y)

    private float xRotation = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        // La camara mira arriba/abajo
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // El cuerpo del player rota izquierda/derecha
        playerBody.Rotate(Vector3.up * mouseX);
    }
}