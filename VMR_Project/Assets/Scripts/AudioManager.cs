using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    public AudioClip clickButton;

    // Plays the button click sound effect
    public void ButtonClickSound()
    {
        if (SFXSource != null && clickButton != null)
        {
            SFXSource.PlayOneShot(clickButton);
        }
    }
}
