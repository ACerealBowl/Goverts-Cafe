using UnityEngine;

public class LogoMainMenu : MonoBehaviour
{
public Animator PersonAnimator;
public string  animationName = "LogoMainMenu";
private void Start()
{
personAnimator = GetComponent<Animator>();
StartCoroutine(PlayAnimation());
}

private IEnumarator
{
if(personAnimator.HasState(0, Animator.StringToHash(animationName)))
{
yield return new WaitForSeconds(8f);
personAnimator.Play(animationName, 0);
yield return new WairForSeconds(personAnimator.GetCurrentAnimatorStateInfo(0).lenght);
}



