using UnityEngine;

public class MovableCup : MonoBehaviour
{
    private CupSystem cupSystem;

    private void Awake()
    {
        cupSystem = FindObjectOfType<CupSystem>();
    }

    private void OnMouseDown()
    {
        if (IsOverDishwasher())
        {
            if (TryGetDishwasherAnimator(out Animator dishwasherAnimator))
            {
                dishwasherAnimator.SetTrigger("Wash");
            }

            if (cupSystem != null)
            {
                cupSystem.ProcessCup();
            }
        }
    }

    private bool IsOverDishwasher()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.zero);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.name.ToLower().Contains("dishwasher"))
            {
                return true;
            }
        }
        return false;
    }

    private bool TryGetDishwasherAnimator(out Animator animator)
    {
        animator = null;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.zero);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.name.ToLower().Contains("dishwasher"))
            {
                animator = hit.collider.GetComponent<Animator>();
                return animator != null;
            }
        }
        return false;
    }
}