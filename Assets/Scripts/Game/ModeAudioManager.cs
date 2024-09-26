using UnityEngine;

public class ModeAudioManager : MonoBehaviour
{
    [Header("---balls---")]
    [SerializeField] AudioSource musicSource;
    [Header("--clips---")]
    public AudioClip background;
    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }
}