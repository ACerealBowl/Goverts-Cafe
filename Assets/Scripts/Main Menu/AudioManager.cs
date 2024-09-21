using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("---balls---")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [Header("--clips---")]
    public AudioClip background;
    public AudioClip step;
    public AudioClip uiHoverSound;  // added for UI hover and click
    public AudioClip uiClickSound;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void StopSFX()
    {
        SFXSource.Stop();
    }

    // New method for UI hover sound
    public void PlayUIHoverSound()
    {
        PlaySFX(uiHoverSound);
    }

    // New method for UI click sound
    public void PlayUIClickSound()
    {
        PlaySFX(uiClickSound);
    }
}