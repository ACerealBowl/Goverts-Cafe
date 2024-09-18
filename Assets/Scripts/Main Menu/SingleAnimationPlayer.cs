using UnityEngine;
using System.Collections;

public class SingleAnimationPlayer : MonoBehaviour
{
    public Animator personAnimator;
    public string animationName = "Person Walking";
    public float minWaitTime = 2f;
    public float maxWaitTime = 5f;
    AudioManager audioManager;

    private void Awake() // dont forget to take dis wit you when copy audio
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>(); 
   
    }

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
            yield return new WaitForSeconds(8f);
            // Wait for a random amount of time
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);

            // Check if the animation exists
            if (personAnimator.HasState(0, Animator.StringToHash(animationName)))
            {
                // Play the animation and sound 
                personAnimator.Play(animationName, 0);
                audioManager.PlaySFX(audioManager.step);
                // Wait for finish
                // yield return new WaitForSeconds(personAnimator.GetCurrentAnimatorStateInfo(0).length); [scrapped]
                yield return new WaitForSeconds(3f);
                audioManager.StopSFX();
            }
            else
            {
                Debug.LogError($"Animation state '{animationName}' not found in the Animator Controller!");
                yield break; // Exit the coroutine if the animation is not found
            }
        }
    }
}