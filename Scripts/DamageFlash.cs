using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DamageFlash : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private EnemyHealth enemyHealth;

    [Header("Flash")]
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private float flashDuration = 0.1f;

    private Renderer[] renderers;
    private Material[][] originalMaterials;
    private Material flashMaterial;

    private float previousHealth;
    private bool initialized;
    private Coroutine flashRoutine;

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();

        // Guardamos los materiales originales de cada renderer, tal cual estan
        originalMaterials = new Material[renderers.Length][];
        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].sharedMaterials;
        }

        // Material blanco simple, funciona sin importar el shader de los materiales originales
        flashMaterial = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
        flashMaterial.color = flashColor;
    }

    private void OnEnable()
    {
        if (enemyHealth != null)
        {
            enemyHealth.OnHealthChanged += HandleHealthChanged;
        }
    }

    private void OnDisable()
    {
        if (enemyHealth != null)
        {
            enemyHealth.OnHealthChanged -= HandleHealthChanged;
        }
    }

    private void HandleHealthChanged(float current, float max)
    {
        if (!initialized)
        {
            previousHealth = current;
            initialized = true;
            return;
        }

        if (current < previousHealth)
        {
            if (flashRoutine != null)
            {
                StopCoroutine(flashRoutine);
            }
            flashRoutine = StartCoroutine(Flash());
        }

        previousHealth = current;
    }

    private IEnumerator Flash()
    {
        // Reemplazamos todos los materiales por el blanco
        for (int i = 0; i < renderers.Length; i++)
        {
            Material[] whiteSet = new Material[originalMaterials[i].Length];
            for (int m = 0; m < whiteSet.Length; m++)
            {
                whiteSet[m] = flashMaterial;
            }
            renderers[i].materials = whiteSet;
        }

        yield return new WaitForSeconds(flashDuration);

        // Devolvemos los materiales originales
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].materials = originalMaterials[i];
        }

        flashRoutine = null;
    }
}