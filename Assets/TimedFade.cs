using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class DelayedCameraTransition : MonoBehaviour
{
    public float transitionDuration = 1f;
    public float initialVisibleDelay = 8f;
    public Color transitionColor = Color.black;

    private Texture2D transitionTexture;
    private float transitionAmount = 0f;  // Start fully visible
    private bool isTransitioning = false;
    private float targetTransitionAmount;

    private void Start()
    {
        transitionTexture = new Texture2D(1, 1);
        transitionTexture.SetPixel(0, 0, Color.white);
        transitionTexture.Apply();

        // Start the transition sequence after a delay
        StartCoroutine(DelayedTransitionSequence());
    }

    private void OnGUI()
    {
        if (transitionAmount > 0)
        {
            Color tempColor = GUI.color;
            GUI.color = new Color(transitionColor.r, transitionColor.g, transitionColor.b, transitionAmount);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), transitionTexture);
            GUI.color = tempColor;
        }
    }

    private void Update()
    {
        if (isTransitioning)
        {
            transitionAmount = Mathf.MoveTowards(transitionAmount, targetTransitionAmount, Time.deltaTime / transitionDuration);
            if (transitionAmount == targetTransitionAmount)
            {
                isTransitioning = false;
            }
        }
    }

    private IEnumerator DelayedTransitionSequence()
    {
        // Wait for the initial delay
        yield return new WaitForSeconds(initialVisibleDelay);

        // Transition to black
        yield return StartCoroutine(BeginTransition(0f, 1f));

        // Optional: Add a small delay while fully black
        yield return new WaitForSeconds(0.5f);

        // Transition back to visible
        yield return StartCoroutine(BeginTransition(1f, 0f));
    }

    private IEnumerator BeginTransition(float startAlpha, float targetAlpha)
    {
        transitionAmount = startAlpha;
        targetTransitionAmount = targetAlpha;
        isTransitioning = true;

        while (isTransitioning)
        {
            yield return null;
        }
    }

    // Call this method to transition to black
    public void TransitionToBlack()
    {
        StartCoroutine(BeginTransition(0f, 1f));
    }

    // Call this method to transition to visible
    public void TransitionToVisible()
    {
        StartCoroutine(BeginTransition(1f, 0f));
    }
}