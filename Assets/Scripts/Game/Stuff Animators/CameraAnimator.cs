using UnityEngine;
using System.Collections;

public class CameraAnimator : MonoBehaviour
{
    public Animator cameraAnimator;
    public PublicFade publicFade;
    private SpriteRenderer[] plateRenderers;
    public Plates plateFinder;
    public Animator Platorz;
    public Animator cupsAnimator;
    public bool DishesMenu = false;
    [SerializeField] private CupSystem cupSystem;

    private void Start()
    {
        GameObject platesObject = GameObject.FindGameObjectWithTag("Plates");
        plateRenderers = platesObject.GetComponentsInChildren<SpriteRenderer>(true);
        StartCoroutine(InitialSetup());
    }

    private IEnumerator InitialSetup()
    {
        StartCoroutine(HidePlates());
        Platorz.SetTrigger("idle");

        // Ensure cups are hidden at start
        yield return new WaitForSeconds(0.1f); // Small delay to ensure animation system is ready
        StartCoroutine(HideCupsSigma());
        DishesMenu = false;
    }

    public void UNPLATEme()
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
        StartCoroutine(ShowCupsSigma());
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

    public void FortniteCups()
    {
        StartCoroutine(HideCupsSigma());
        DishesMenu = false;
    }

    private IEnumerator HideCupsSigma()
    {
        yield return new WaitForSeconds(0.5f);
        cupsAnimator.SetTrigger("Hide");
    }

    private IEnumerator ShowCupsSigma()
    {
        yield return new WaitForSeconds(0.5f);
        if (!cupSystem.HasNoCups)
        {
            cupsAnimator.SetTrigger("Show");
            DishesMenu = true;
        }
        else
        {
            cupsAnimator.SetTrigger("Hide");
            DishesMenu = false;
        }
    }

    private bool GetRequiredCups()
    {
        return !cupSystem.HasNoCups;
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