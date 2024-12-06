using UnityEngine;

public class AnimationSounds : MonoBehaviour
{
    [SerializeField] AudioClip walkSound;
    [SerializeField] AudioClip runSound;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] private AudioSource audioSource; // Assign an AudioSource component in the inspector

    // Methods to play sounds
    public void PlayWalkSound()
    {
        if (audioSource != null && walkSound != null)
        {
            audioSource.clip = walkSound; // Set the  sound clip
            audioSource.Play();           // Play the sound
        }
    }

    public void PlayRunSound()
    {
        if (audioSource != null && runSound != null)
        {
            audioSource.clip = runSound; 
            audioSource.Play();          
        }
    }

    public void PlayJumpSound()
    {
        if (audioSource != null && jumpSound != null)
        {
            audioSource.clip = jumpSound; 
            audioSource.Play();         
        }
    }
}
