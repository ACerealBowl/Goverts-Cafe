using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GovertMainMenu : MonoBehaviour
{
    public Animator personAnimator;
    public string animationName = "GovertMainMenu";

    private void Start()
    {
        if (personAnimator == null)
        {
            personAnimator = GetComponent<Animator>();
        }
        if (personAnimator == null)
        {
            Debug.LogError("aoleu");
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
            // Wait to finish
            yield return new WaitForSeconds(personAnimator.GetCurrentAnimatorStateInfo(0).length);
        }
    }
}