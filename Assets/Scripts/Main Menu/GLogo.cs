using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LogoMainMenu : MonoBehaviour
{
    public Animator personAnimator;
    public string animationName = "LogoMainMenu";
    private void Start()
    {
        personAnimator = GetComponent<Animator>();
        StartCoroutine(PlayAnimation());
    }

    private IEnumerator PlayAnimation()
    {
        if (personAnimator.HasState(0, Animator.StringToHash(animationName)))
        {
            yield return new WaitForSeconds(8f);
            personAnimator.Play(animationName, 0);
            yield return new WaitForSeconds(personAnimator.GetCurrentAnimatorStateInfo(0).length);
        }
    }
}


