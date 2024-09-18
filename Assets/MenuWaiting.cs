using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuWaiting : MonoBehaviour
{
    public Animator personAnimator;
    public string animationName = "Main menu n";

    private void Start()
    {
        if (personAnimator == null)
        {
            personAnimator = GetComponent<Animator>();
        }
        if (personAnimator == null)
        {
            Debug.LogError("Animator component not found!");
            return;
        }
        StartCoroutine(PlayAnimation());
    }

    private IEnumerator PlayAnimation()
    {
        // Check if the animation exists
        if (personAnimator.HasState(0, Animator.StringToHash(animationName)))
        {
            yield return new WaitForSeconds(8);
            // Play the animation
            personAnimator.Play(animationName, 0);
            // Wait for the animation to finish
            yield return new WaitForSeconds(personAnimator.GetCurrentAnimatorStateInfo(0).length);
        }
        else
        {
            Debug.LogError($"Animation state '{animationName}' not found in the Animator Controller!");
            yield break; // Exit the coroutine if the animation is not found
        }
    }
}