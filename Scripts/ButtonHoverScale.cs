using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Efecto hover")]
    [SerializeField] private float hoverScale = 1.15f; // 1.15 = 15% mas grande
    [SerializeField] private float smoothSpeed = 8f;    // que tan rapido llega al tamano objetivo

    private Vector3 initialScale;
    private Vector3 targetScale;

    private void Awake()
    {
        initialScale = transform.localScale;
        targetScale = initialScale;
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, smoothSpeed * Time.deltaTime);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = initialScale * hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = initialScale;
    }
}