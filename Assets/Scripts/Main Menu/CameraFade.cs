using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Camera))]
public class CameraFade : MonoBehaviour
{
    public float fadeDuration = 1f;
    public Color fadeColor = Color.black;

    private Texture2D fadeTexture;
    private float fadeAmount = 1f;  // Start fully faded out
    private bool isFading = false;
    public float FadeDelay = 8f;
    private void Start()
    {
        fadeTexture = new Texture2D(1, 1);
        fadeTexture.SetPixel(0, 0, Color.white);
        fadeTexture.Apply();
        
        // Start fading in immediately
        StartCoroutine (StartFading(1f, 0f));
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
            }
        }
    }

    private float targetFadeAmount;

    public IEnumerator StartFading(float startAlpha, float targetAlpha)
    {
        yield return new WaitForSeconds(FadeDelay);
        fadeAmount = startAlpha;
        targetFadeAmount = targetAlpha;
        isFading = true;
    }

    // Call this method to fade out
    public void FadeOut()
    {
        StartFading(0f, 1f);
    }

    // Call this method to fade in
    public void FadeIn()
    {
        StartFading(1f, 0f);
    }
}