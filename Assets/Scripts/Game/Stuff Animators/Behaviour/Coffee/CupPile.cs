using UnityEngine;
using System.Collections;

public class CupPile : MonoBehaviour
{
    [SerializeField] private GameObject emptyCleanCupPrefab;
    [SerializeField] private CupSystem cupSystem;
    [SerializeField] private Animator cupPileAnimator;

    private void Start()
    {
        if (cupSystem == null)
        {
            cupSystem = FindObjectOfType<CupSystem>();
            if (cupSystem == null)
            {
                Debug.LogError("CupSystem not found in the scene!");
            }
        }

        if (cupPileAnimator == null)
        {
            cupPileAnimator = GetComponent<Animator>();
        }

        UpdateVisibility();
    }

    private void Update()
    {
        UpdateVisibility();
    }

    private void UpdateVisibility()
    {
        // Check if all cups are dirty and hide the clean cup pile if needed
        if (cupSystem != null && cupSystem.DeadCups)
        {
            if (cupPileAnimator != null)
            {
                cupPileAnimator.SetTrigger("Hide");
            }
        }
    }

    private void OnMouseDown()
    {
        if (cupSystem != null && cupSystem.DeadCups)
        {
            // Can't get new cups when all cups are dirty
            Debug.Log("No clean cups available!");
            return;
        }

        if (SingularCup.currentlyHeldCup != null) // Using static field instead of property
        {
            Debug.Log("Already holding a cup!");
            return;
        }

        // Create a new clean cup
        GameObject newCup = Instantiate(emptyCleanCupPrefab, transform.position, Quaternion.identity);
        SingularCup cupScript = newCup.GetComponent<SingularCup>();
        if (cupScript != null)
        {
            cupScript.PickUp();
            Debug.Log("Picked up a clean cup from the pile");
        }
    }
}