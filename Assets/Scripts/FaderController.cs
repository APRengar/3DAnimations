using UnityEngine;
using UnityEngine.Events;

public class FaderController : MonoBehaviour
{
    public static FaderController Instance { get; private set; }

    [SerializeField] private CanvasGroup canvasGroup; // Reference to the CanvasGroup component
    [SerializeField] private float fadeDuration = 1f; // Duration of the fade effect
    
    // UnityEvents to allow triggering fade-in and fade-out from other MonoBehaviors
    public UnityEvent OnFadeIn;
    public UnityEvent OnFadeOut;

    private void Awake()
    {
        // Implement the singleton pattern: if an instance already exists, destroy this one
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy this instance if one already exists
        }
        else
        {
            Instance = this; // Set the singleton instance
            DontDestroyOnLoad(gameObject); // Optional: Keep the Fader across scenes
        }
        // Optional: Ensure the CanvasGroup is set up correctly
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
        // Initialize the UnityEvents if they haven't been set in the inspector
        if (OnFadeIn == null)
            OnFadeIn = new UnityEvent();

        if (OnFadeOut == null)
            OnFadeOut = new UnityEvent();
    }
    private void Start() 
    {
        OnFadeIn.AddListener(FadeIn);
        OnFadeOut.AddListener(FadeOut);
    }

    public void FadeIn()
    {
        StartCoroutine(Fade(0, 1)); // Fade from transparent to visible
    }

    public void FadeOut()
    {
        StartCoroutine(Fade(1, 0)); // Fade from visible to transparent
    }

    private System.Collections.IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, timer / fadeDuration);
            yield return null; // Wait for the next frame
        }
        // Ensure the final alpha is set
        canvasGroup.alpha = endAlpha;
        // Optionally disable the CanvasGroup when fully faded out
        if (endAlpha == 0)
            canvasGroup.interactable = canvasGroup.blocksRaycasts = false;
        else
            canvasGroup.interactable = canvasGroup.blocksRaycasts = true;
    }
}
