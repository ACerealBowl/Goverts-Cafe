using UnityEngine;
using System.Collections;

public class CameraAnimator : MonoBehaviour
{
    public Animator cameraAnimator;
    public PublicFade publicFade;
    private SpriteRenderer[] plateRenderers;
    public Plates plateFinder;
    public Animator Platorz;

    private void Start()
    {
        GameObject platesObject = GameObject.FindGameObjectWithTag("Plates");
        plateRenderers = platesObject.GetComponentsInChildren<SpriteRenderer>(true);
        StartCoroutine(HidePlates());
        Platorz.SetTrigger("idle");

    }
    public void UNPLATEme ()
    {
        StartCoroutine(HidePlates());
    }
    private IEnumerator ShowPlates()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (SpriteRenderer renderer in plateRenderers)
        {
            renderer.enabled = true;
        }
    }

    public IEnumerator HidePlates()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (SpriteRenderer renderer in plateRenderers)
        {
            renderer.enabled = false;
        }
    }

    public void PlayDishesAnimation()
    {
        StartCoroutine(AnimationSequence("DishesView"));
        StartCoroutine(HidePlates());
    }

    public void PlayCoffeeAnimation()
    {
        StartCoroutine(AnimationSequence("CoffeeView"));
        StartCoroutine(ShowPlates());
        Platorz.SetTrigger("idle");
    }

    public void PlayPastryAnimation()
    {
        StartCoroutine(AnimationSequence("PastryView"));
        StartCoroutine(ShowPlates());
        Platorz.SetTrigger("Pastry");
    }

    public void PlayCashRegisterAnimation()
    {
        StartCoroutine(AnimationSequence("CashRegisterView"));
        StartCoroutine(ShowPlates());
        Platorz.SetTrigger("CashRegister");
    }

    private IEnumerator AnimationSequence(string triggerName)
    {
        cameraAnimator.SetTrigger(triggerName);
        publicFade.FadeOut();
        yield return new WaitForSeconds(publicFade.fadeDuration);
        cameraAnimator.SetTrigger("idle");
        yield return new WaitForSeconds(0.3f);
        publicFade.FadeIn();
    }
}