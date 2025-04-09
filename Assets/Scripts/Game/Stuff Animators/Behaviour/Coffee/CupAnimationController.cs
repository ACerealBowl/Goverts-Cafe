using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CupAnimationController : MonoBehaviour
{
    private Animator cupAnimator;
    private CameraAnimator cameraAnimator;
    private FilledCup filledCup;
    private SingularCup singularCup;

    private void Awake()
    {
        cupAnimator = GetComponent<Animator>();
        cameraAnimator = CameraAnimator.Instance;
        filledCup = GetComponent<FilledCup>();
        singularCup = GetComponent<SingularCup>();
    }

    private void Start()
    {
        UpdateCupAnimation();
    }

    private void Update()
    {
        // Only update if the cup is placed (not being held)
        if ((filledCup != null && !filledCup.IsHeld()) ||
            (singularCup != null && !singularCup.IsHeld()))
        {
            UpdateCupAnimation();
        }
    }

    private void UpdateCupAnimation()
    {
        if (cupAnimator == null || cameraAnimator == null) return;

        // Show cup in CoffeeView, hide in other views
        if (cameraAnimator.IsInCoffeeViewActive || !cameraAnimator.IsPastryViewActive)
        {
            cupAnimator.SetTrigger("idle");
        }
        else
        {
            cupAnimator.SetTrigger("Hide");
        }
    }

    // Called when the cup is picked up to ensure it's visible
    public void OnCupPickedUp()
    {
        if (cupAnimator != null)
        {
            cupAnimator.SetTrigger("idle");
        }
    }

    // Called when the cup is placed down
    public void OnCupPlaced()
    {
        UpdateCupAnimation();
    }
}