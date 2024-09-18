using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("---balls---")]
    [SerializeField]  AudioSource musicSource;
    [SerializeField]  AudioSource SFXSource;
    [Header("--clips---")]
    public AudioClip background;
    public AudioClip step;
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
}
