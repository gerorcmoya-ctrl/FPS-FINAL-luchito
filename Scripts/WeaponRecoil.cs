using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    [Header("Recoil")]
    [SerializeField] private float recoilDistance = 0.05f; // cuanto se mueve hacia atras
    [SerializeField] private float recoilRotation = 5f;    // cuanto se inclina hacia arriba
    [SerializeField] private float recoilSpeed = 10f;       // que tan rapido retrocede
    [SerializeField] private float returnSpeed = 6f;        // que tan rapido vuelve a su lugar

    private Vector3 initialLocalPos;
    private Quaternion initialLocalRot;
    private Vector3 targetPos;
    private Quaternion targetRot;

    private void Awake()
    {
        initialLocalPos = transform.localPosition;
        initialLocalRot = transform.localRotation;
        targetPos = initialLocalPos;
        targetRot = initialLocalRot;
    }

    private void Update()
    {
        // Vuelve suavemente a la posicion/rotacion original
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, returnSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRot, returnSpeed * Time.deltaTime);
    }

    // Llamado desde el script de disparo cada vez que se dispara
    public void DoRecoil()
    {
        transform.localPosition = initialLocalPos - new Vector3(0f, 0f, recoilDistance);
        transform.localRotation = initialLocalRot * Quaternion.Euler(-recoilRotation, 0f, 0f);

        targetPos = initialLocalPos;
        targetRot = initialLocalRot;
    }
}