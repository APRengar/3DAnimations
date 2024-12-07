using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject particlesPrefab;
    [SerializeField] AudioClip interactionsound;
    [SerializeField] AudioSource audioSource;

    [Header ("Materials")]
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material highlightMaterial;

    private Renderer objectRenderer;
    
    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer == null)
        {
            Debug.LogError("Interactable object is missing a Renderer component.");
        }
    }

    public void RaycastHit(Vector3 hitPosition)
    {
        if (animator)
        {
            animator.SetTrigger("Interact");
        }
        if (particlesPrefab)
        {
            Instantiate(particlesPrefab, hitPosition, Quaternion.identity);
        }
        if (interactionsound && audioSource)
        {
            audioSource.PlayOneShot(interactionsound);
        }
    }

    public void SetHighlight(bool isHighlighted)
    {
        if (objectRenderer != null)
        {
            objectRenderer.material = isHighlighted ? highlightMaterial : defaultMaterial;
        }
    }
}
