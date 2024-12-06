using System.Collections;
using DigitalRuby.RainMaker;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeatherController : MonoBehaviour
{
    [SerializeField] private Button toggleFogButton;
    
    [SerializeField] private Slider rainSlider; 
    [SerializeField] private RainScript rainPrefab;
    [SerializeField] private Material skyboxBelowThreshold; //Equal sunny
    [SerializeField] private Material skyboxAboveThreshold; //Equal rainy
    private bool isCoroutineRunning = false; // Prevent multiple coroutine calls
    private float weatherChangeThreshold = 0.1f; // Slider threshold
    private float sliderPreviousValue = 0f;

    private void Start()
    {
        //Start behaviors
        RenderSettings.fog = false;

        //Link buttons
        if (toggleFogButton != null)
            toggleFogButton.onClick.AddListener(ToggleFog);

        //Link rain slider
        if (rainSlider != null)
            rainSlider.onValueChanged.AddListener(OnSliderValueChanged);
        
    }

    private void OnSliderValueChanged(float currentValue)
    {
        // Trigger coroutine logic only when crossing the threshold
        rainPrefab.RainIntensity = currentValue;
        if (isCoroutineRunning) return; // Prevent triggering during an active coroutine

        // Check if the slider crossed the threshold
        if (sliderPreviousValue < weatherChangeThreshold && currentValue >= weatherChangeThreshold)
        {
            // Crossing from below to above the threshold
            StartCoroutine(SwitchWeather(skyboxAboveThreshold));
        }
        else if (sliderPreviousValue > weatherChangeThreshold && currentValue <= weatherChangeThreshold)
        {
            // Crossing from above to below the threshold
            StartCoroutine(SwitchWeather(skyboxBelowThreshold));
        }

        // Update the previous value
        sliderPreviousValue = currentValue;
    }

    private void ToggleFog()
    {
        // Toggle the RenderSettings.fog (true or false)
        RenderSettings.fog = !RenderSettings.fog;
        
        // Optionally, you can update the button text to show the current state
        if (RenderSettings.fog)
        {
            toggleFogButton.GetComponentInChildren<TextMeshProUGUI>().text = "Disable Fog";
        }
        else
        {
            toggleFogButton.GetComponentInChildren<TextMeshProUGUI>().text = "Enable Fog";
        }
    }

    private IEnumerator SwitchWeather(Material newMaterial)
    {
        isCoroutineRunning = true; // Prevent multiple coroutine calls
        rainSlider.interactable = false;
        // Fade in
        FaderController.Instance.OnFadeIn.Invoke();
        yield return new WaitForSeconds(FaderController.Instance.FadeDuration);
        // Change skybox
        ChangeSkybox(newMaterial);
        // Fade out
        FaderController.Instance.OnFadeOut.Invoke();
        yield return new WaitForSeconds(FaderController.Instance.FadeDuration);
        rainSlider.interactable = true;
        isCoroutineRunning = false; // Allow coroutines to run again
    }

    private void ChangeSkybox(Material newSkybox)
    {
        RenderSettings.skybox = newSkybox;

        // Ensure the changes are reflected immediately by reapplying lighting settings
        DynamicGI.UpdateEnvironment();
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent errors
        if (toggleFogButton != null)
            toggleFogButton.onClick.RemoveListener(ToggleFog);
        
        if (rainSlider != null)
            rainSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }
}
