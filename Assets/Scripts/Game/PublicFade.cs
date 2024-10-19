using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PublicFade : MonoBehaviour
{
    public float fadeDuration = 1f;
    public Color fadeColor = Color.black;
    public float initialFadeDelay = 8f;

    private Texture2D fadeTexture;
    private float fadeAmount = 1f;  // Start fully faded out
    private bool isFading = false;
    private float targetFadeAmount;

    private void Start()
    {
        fadeTexture = new Texture2D(1, 1);
        fadeTexture.SetPixel(0, 0, Color.white);
        fadeTexture.Apply();

        // Start initial fade-in
        StartCoroutine(InitialFadeIn());
    }

    private void OnGUI()
    {
        if (fadeAmount > 0)
        {
            Color tempColor = GUI.color;
            GUI.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, fadeAmount);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
            GUI.color = tempColor;
        }
    }

    private void Update()
    {
        if (isFading)
        {
            fadeAmount = Mathf.MoveTowards(fadeAmount, targetFadeAmount, Time.deltaTime / fadeDuration);
            if (fadeAmount == targetFadeAmount)
            {
                isFading = false;
                fadeDuration = 0.5f;
            }
        }
    }

    private IEnumerator InitialFadeIn()
    {
        yield return new WaitForSeconds(initialFadeDelay);
        StartFading(1f, 0f);
    }

    private void StartFading(float startAlpha, float targetAlpha)
    {
        fadeAmount = startAlpha;
        targetFadeAmount = targetAlpha;
        isFading = true;
    }

    // Call this method to fade out
    public void FadeOut()
    {
        StartFading(fadeAmount, 1f);
    }

    // Call this method to fade in
    public void FadeIn()
    {
        StartFading(fadeAmount, 0f);
    }

    public void SpeedFadeOut()
    {
        fadeDuration = 0.2f;
        StartFading(fadeAmount, 1f);
    }

    public void SpeedFadeIn()
    {
        fadeDuration = 0.2f;
        StartFading(fadeAmount, 0f);
    }
}