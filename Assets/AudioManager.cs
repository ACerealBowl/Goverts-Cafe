using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("---balls---")]
    [SerializeField]  AudioSource musicSource;
    [SerializeField]  AudioSource sfxSource;
    [Header("--clips---")]
    public AudioClip background;
    public AudioClip step;
    void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }
}
