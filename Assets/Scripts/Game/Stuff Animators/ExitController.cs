using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ExitController : MonoBehaviour
{
    [SerializeField] private GameObject main;
    [SerializeField] private GameObject dishes;
    [SerializeField] private GameObject coffee;
    [SerializeField] private GameObject pastry;
    [SerializeField] private GameObject cashRegister;
    public PublicFade publicFade;

    public void TriggerExit()
    {
        StartCoroutine(ExitSequence());
    }

    public void TriggerBalls()
    {
        StartCoroutine(FadeIn());
    }

    // Disabling game objects
    public void DisableDishes()
    {
        StartCoroutine(DisableObject(dishes));
    }

    public void DisableCoffee()
    {
        StartCoroutine(DisableObject(coffee));
    }

    public void DisablePastry()
    {
        StartCoroutine(DisableObject(pastry));
    }

    public void DisableCashRegister()
    {
        StartCoroutine(DisableObject(cashRegister));
    }

    private IEnumerator DisableObject(GameObject objectToDisable)
    {
        yield return new WaitForSeconds(0.5f);
        objectToDisable.SetActive(false);
        yield return null;
    }

    //Transition
    private IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(0.5f);
        publicFade.SpeedFadeIn();
    }

    private IEnumerator ExitSequence()
    {
        publicFade.SpeedFadeOut();
        yield return new WaitForSeconds(publicFade.fadeDuration);
        main.SetActive(true);
    }
}