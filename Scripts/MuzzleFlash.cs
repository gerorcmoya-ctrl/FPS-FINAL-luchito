using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class MuzzleFlash : MonoBehaviour
{
    private ParticleSystem flashParticles;

    private void Awake()
    {
        flashParticles = GetComponent<ParticleSystem>();
    }

    public void Play()
    {
        Debug.Log("MuzzleFlash.Play() llamado en el frame: " + Time.frameCount);
        flashParticles.Stop();
        flashParticles.Play();
    }
}