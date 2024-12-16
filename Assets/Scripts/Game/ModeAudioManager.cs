using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ModeAudioManager : MonoBehaviour
{
    [Header("---Audio Source---")]
    [SerializeField] AudioSource musicSource;

    [Header("---Audio Clips---")]
    public AudioClip background;

    [Header("---Settings---")]
    public float fadeOutDuration = 2f; // Public fade-out duration (2 seconds by default)

    // Singleton Instance
    public static ModeAudioManager Instance;

    private void Awake()
    {
        // Ensure only one instance of ModeAudioManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Don't destroy on scene load
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
        }
    }

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    // Fade-out method
    public void FadeOutMusicAndSwitchScene(string sceneName)
    {
        StartCoroutine(FadeOutAndLoadScene(sceneName));
    }

    private IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        float startVolume = musicSource.volume;

        // Fade-out over the duration
        while (musicSource.volume > 0)
        {
            musicSource.volume -= startVolume * Time.deltaTime / fadeOutDuration;
            yield return null; // Wait for the next frame
        }

        musicSource.Stop(); // Stop the music after fading out
        musicSource.volume = startVolume; // Reset volume for future use

        // Load the new scene after the fade-out is complete
        SceneManager.LoadScene(sceneName);
    }
}
