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
    public Animator CoffeeAnimator;
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
        CoffeeAnimator.SetTrigger("Hidden");

        // Ensure cups are hidden at start
        yield return new WaitForSeconds(0.1f); // Small delay to ensure animation system is ready
        StartCoroutine(HideCupsSigma());
        DishesMenu = false;
    }

    public void UNPLATEme()
    {
        StartCoroutine(HidePlates());
    }
    public void FortniteCups()
    {
        StartCoroutine(HideCupsSigma());
        DishesMenu = false;
    }

    public void HideCoffeeMachine()
    {
        StartCoroutine (hideCoffee());
    }

    private IEnumerator ShowPlates()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (SpriteRenderer renderer in plateRenderers)
        {
            renderer.enabled = true;
        }
    }
    private IEnumerator hideCoffee()
    {
        yield return new WaitForSeconds(0.5f);
        CoffeeAnimator.SetTrigger("Hidden");
    }

    public IEnumerator HidePlates()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (SpriteRenderer renderer in plateRenderers)
        {
            renderer.enabled = false;
        }
    }
    private IEnumerator HideCupsSigma()
    {
        yield return new WaitForSeconds(0.5f);
        cupsAnimator.SetTrigger("Hide");
    }
    private IEnumerator ShowCoffee()
    {
        yield return new WaitForSeconds(0.5f);
        CoffeeAnimator.SetTrigger("idle");
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
        StartCoroutine (ShowCoffee());
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