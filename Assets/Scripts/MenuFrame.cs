using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFrame : MonoBehaviour
{
    public float fadeDuration = 1f;
    public Color fadeColor = Color.black;
    private Texture2D fadeTexture;
    private float fadeAmount = 0f;
    private bool isFading = false;
    private bool Endler = false;

    private void Start()
    {
        fadeTexture = new Texture2D(1, 1);
        fadeTexture.SetPixel(0, 0, Color.white);
        fadeTexture.Apply();
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
                if (fadeAmount == 1f)
                {
                    if (Endler)
                    SceneManager.LoadSceneAsync("Endless");
                }
            }
        }
    }

    private float targetFadeAmount;

    private IEnumerator StartFading(float startAlpha, float targetAlpha)
    {
        fadeAmount = startAlpha;
        targetFadeAmount = targetAlpha;
        isFading = true;
        yield return new WaitForSeconds(fadeDuration);
    }

    public void FadeOut()
    {
        StartCoroutine(StartFading(0f, 1f));
    }

    public void Story()
    {
        Application.Quit();
    }
    public void Endless()
    {
        Endler=true;
        FadeOut();
    }
    public void Race()
    {
        Application.Quit();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}