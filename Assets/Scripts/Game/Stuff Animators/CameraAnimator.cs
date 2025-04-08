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
    public Animator PastryAnimator;
    public bool DishesMenu = false;
    [SerializeField] private CupSystem cupSystem;
    [SerializeField] private Pastry pastrySystem;
    [SerializeField] private PlateAnimationController plateAnimController;

    public static CameraAnimator Instance;
    public bool IsPastryViewActive { get; private set; }
    // Pastry scale values for different views
    private const float PASTRY_SCALE_CASH_REGISTER = 0.12f;
    private const float PASTRY_SCALE_COFFEE = 0.15f;
    private const float PASTRY_SCALE_DEFAULT = 0.18f; // Default scale from Pastry.cs

    private void Awake() // Add this method
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        GameObject platesObject = GameObject.FindGameObjectWithTag("Plates");
        plateRenderers = platesObject.GetComponentsInChildren<SpriteRenderer>(true);

        if (pastrySystem == null)
        {
            pastrySystem = FindObjectOfType<Pastry>();
            if (pastrySystem == null)
            {
                Debug.LogWarning("Pastry system not found in scene!");
            }
        }

        if (plateAnimController == null)
        {
            plateAnimController = FindObjectOfType<PlateAnimationController>();
            if (plateAnimController == null)
            {
                Debug.LogWarning("PlateAnimationController not found in scene!");
            }
        }

        StartCoroutine(InitialSetup());
    }

    private IEnumerator InitialSetup()
    {
        StartCoroutine(HidePlates());
        Platorz.SetTrigger("idle");
        CoffeeAnimator.SetTrigger("Hidden");
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(HideCupsSigma());
        DishesMenu = false;
        StartCoroutine(hidePastry());
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
        StartCoroutine(hideCoffee());
    }

    public void HidePastry()
    {
        StartCoroutine(hidePastry());
    }

    private IEnumerator ShowPlates()
    {
        yield return new WaitForSeconds(0.5f);
        if (plateAnimController != null)
        {
            plateAnimController.SetPlatesAndPastriesVisibility(true);
        }
        else
        {
            foreach (SpriteRenderer renderer in plateRenderers)
            {
                renderer.enabled = true;
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.SetAllPlatesVisibility(true);
            }
        }
    }

    private IEnumerator hideCoffee()
    {
        yield return new WaitForSeconds(0.5f);
        CoffeeAnimator.SetTrigger("Hidden");
    }

    private IEnumerator hidePastry()
    {
        yield return new WaitForSeconds(0.5f);
        PastryAnimator.SetTrigger("Hidden");

        if (pastrySystem != null)
        {
            pastrySystem.SetVisibility(false);
        }
    }

    public IEnumerator HidePlates()
    {
        yield return new WaitForSeconds(0.5f);

        if (plateAnimController != null)
        {
            plateAnimController.SetPlatesAndPastriesVisibility(false);
        }
        else
        {
            foreach (SpriteRenderer renderer in plateRenderers)
            {
                renderer.enabled = false;
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.SetAllPlatesVisibility(false);
            }
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

        // Scale pastries for coffee view
        ScaleAllPastries(PASTRY_SCALE_COFFEE);
    }

    private IEnumerator ShowPastry()
    {
        yield return new WaitForSeconds(0.5f);
        PastryAnimator.SetTrigger("idle");

        if (pastrySystem != null)
        {
            pastrySystem.SetVisibility(true);
        }

        // Reset to default scale for pastry view
        ScaleAllPastries(PASTRY_SCALE_DEFAULT);
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
        IsPastryViewActive = false;
    }

    public void PlayCoffeeAnimation()
    {
        StartCoroutine(AnimationSequence("CoffeeView"));
        StartCoroutine(ShowPlates());
        StartCoroutine(ShowCoffee());
        Platorz.SetTrigger("idle");
        IsPastryViewActive = false;
    }

    public void PlayPastryAnimation()
    {
        StartCoroutine(AnimationSequence("PastryView"));
        StartCoroutine(ShowPastry());
        StartCoroutine(ShowPlates());
        Platorz.SetTrigger("Pastry");
        IsPastryViewActive = true;
    }

    public void PlayCashRegisterAnimation()
    {
        StartCoroutine(AnimationSequence("CashRegisterView"));
        StartCoroutine(ShowPlates());
        Platorz.SetTrigger("CashRegister");
        IsPastryViewActive = false;
        // Scale pastries for cash register view
        ScaleAllPastries(PASTRY_SCALE_CASH_REGISTER);
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

    // New method to scale all pastries (both placed and unplaced)
    private void ScaleAllPastries(float scale)
    {


        // Scale placed pastries through GameManager
        if (GameManager.Instance != null)
        {
            var allPlates = GameManager.Instance.GetAllPlateContents();
            foreach (var plate in allPlates)
            {
                foreach (var item in plate.Value)
                {
                    if (item.itemObject != null &&
                        (item.itemType == "Cake" || item.itemType == "Donut" ||
                         item.itemType == "Tiramisu" || item.itemType == "Muffin"))
                    {
                        item.itemObject.transform.localScale = new Vector3(scale, scale, scale);
                    }
                }
            }
        }
    }

    // Simplified visibility control methods
    public void HidePlatesAndPastries()
    {
        StartCoroutine(HidePlatesAndPastriesCoroutine());
    }

    private IEnumerator HidePlatesAndPastriesCoroutine()
    {
        yield return new WaitForSeconds(0.5f);

        if (plateAnimController != null)
        {
            plateAnimController.SetPlatesAndPastriesVisibility(false);
        }
        else if (GameManager.Instance != null)
        {
            GameManager.Instance.SetAllPlatesVisibility(false);
        }

        if (pastrySystem != null)
        {
            pastrySystem.SetVisibility(false);
        }

        Platorz.SetTrigger("Hidden");
        PastryAnimator.SetTrigger("Hidden");
    }

    public void ShowPlatesAndPastries()
    {
        StartCoroutine(ShowPlatesAndPastriesCoroutine());
    }

    private IEnumerator ShowPlatesAndPastriesCoroutine()
    {
        yield return new WaitForSeconds(0.5f);

        if (plateAnimController != null)
        {
            plateAnimController.SetPlatesAndPastriesVisibility(true);
        }
        else if (GameManager.Instance != null)
        {
            GameManager.Instance.SetAllPlatesVisibility(true);
        }

        if (pastrySystem != null)
        {
            pastrySystem.SetVisibility(true);
        }

        Platorz.SetTrigger("idle");
        PastryAnimator.SetTrigger("idle");
    }
}