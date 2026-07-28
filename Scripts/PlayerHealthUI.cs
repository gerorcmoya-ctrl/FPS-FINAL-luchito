using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private PlayerHealth playerHealth;

    [Header("Fluidez")]
    [SerializeField] private float smoothSpeed = 3f; // mas alto = baja mas rapido hasta el valor real

    private float targetValue;
    private bool initialized;

    private void Awake()
    {
        // Awake se ejecuta antes que cualquier Start, asi nos aseguramos de estar
        // suscriptos antes de que PlayerHealth avise su valor inicial
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += SetTargetValue;
        }
    }

    private void SetTargetValue(float current, float max)
    {
        slider.maxValue = max;
        targetValue = current;

        if (!initialized)
        {
            // La primera vez (al arrancar el juego) ponemos el valor de una,
            // sin animacion, para que la barra arranque llena y fija
            slider.value = current;
            initialized = true;
        }
    }

    private void Update()
    {
        slider.value = Mathf.MoveTowards(slider.value, targetValue, smoothSpeed * Time.deltaTime);
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= SetTargetValue;
        }
    }
}