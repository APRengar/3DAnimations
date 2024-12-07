using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinFade : MonoBehaviour
{
    private Material defaultMaterial;
    [SerializeField] private float alphaFadeDuration = 0.5f;

    private Material currentMaterial;
    private Renderer objectRenderer;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer == null)
        {
            Debug.LogError("Interactable object is missing a Renderer component.");
            return;
        }
        
        defaultMaterial = objectRenderer.material;
        // Clone the default material to allow independent alpha manipulation
        if (defaultMaterial != null)
        {
            currentMaterial = Instantiate(defaultMaterial);
            objectRenderer.material = currentMaterial;
        }
    }

    public void RaycastHitEvent()
    {
        StartCoroutine(FadeOutAlpha());
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    private IEnumerator FadeOutAlpha()
    {
        if (currentMaterial == null) yield break;
        float elapsedTime = 0f;

        // Get the initial color
        Color initialColor = currentMaterial.color;

        while (elapsedTime < alphaFadeDuration)
        {
            // Linearly interpolate the alpha value
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / alphaFadeDuration);
            currentMaterial.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the material's alpha is set to 0 at the end
        currentMaterial.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
        DestroySelf();
    }
}
