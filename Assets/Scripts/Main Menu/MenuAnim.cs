using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// Simpler Also reusable version of Govert.cs
public class MenuAnim : MonoBehaviour
{
    public Animator personAnimator;
    public string animationName = "TextMenu";
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


