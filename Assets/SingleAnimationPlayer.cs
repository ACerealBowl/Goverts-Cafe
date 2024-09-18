using UnityEngine;
using System.Collections;

public class SingleAnimationPlayer : MonoBehaviour
{
    public Animator personAnimator;
    public string animationName = "Person Walking";
    public float minWaitTime = 2f;
    public float maxWaitTime = 5f;

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
        while (true)
        {
            // Wait for a random amount of time
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);

            // Check if the animation exists
            if (personAnimator.HasState(0, Animator.StringToHash(animationName)))
            {
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
}