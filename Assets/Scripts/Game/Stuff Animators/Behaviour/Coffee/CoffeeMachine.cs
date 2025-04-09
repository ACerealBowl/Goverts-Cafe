using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoffeeMachine : MonoBehaviour
{
    [SerializeField] private Transform[] coffeeSlots;
    [SerializeField] private float slotRadius = 0.5f;
    [SerializeField] private GameObject filledCupPrefab;
    [SerializeField] private float brewTime = 8f;

    private Dictionary<int, GameObject> slotContents = new Dictionary<int, GameObject>();
    private Dictionary<int, Coroutine> brewCoroutines = new Dictionary<int, Coroutine>();

    private void Start()
    {
        // Initialize slot tracking
        for (int i = 0; i < coffeeSlots.Length; i++)
        {
            slotContents[i] = null;
        }
    }

    private void Update()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(0) && SingularCup.currentlyHeldCup != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            // Check if clicking on a slot
            for (int i = 0; i < coffeeSlots.Length; i++)
            {
                if (Vector2.Distance(new Vector2(mousePos.x, mousePos.y),
                                    new Vector2(coffeeSlots[i].position.x, coffeeSlots[i].position.y)) <= slotRadius)
                {
                    // Try to place cup in slot
                    TryPlaceCupInSlot(i);
                    break;
                }
            }
        }
    }

    private void TryPlaceCupInSlot(int slotIndex)
    {
        // Check if slot is already occupied
        if (slotContents[slotIndex] != null)
        {
            Debug.Log($"Coffee slot {slotIndex} is already occupied!");
            return;
        }

        GameObject heldCup = SingularCup.currentlyHeldCup;
        SingularCup cupScript = heldCup.GetComponent<SingularCup>();

        // Check if it's an empty cup
        if (cupScript == null || cupScript.GetCupType() != "EmptyCup")
        {
            Debug.Log("Can only place empty cups in coffee machine!");
            return;
        }

        // Place cup in slot
        heldCup.transform.position = coffeeSlots[slotIndex].position;
        cupScript.PlaceDown();
        slotContents[slotIndex] = heldCup;

        // Start brewing process
        brewCoroutines[slotIndex] = StartCoroutine(BrewCoffee(slotIndex));

        Debug.Log($"Placed cup in coffee slot {slotIndex}, brewing started");
    }

    private IEnumerator BrewCoffee(int slotIndex)
    {
        // Wait for brew time
        Debug.Log($"Brewing coffee in slot {slotIndex}...");
        yield return new WaitForSeconds(brewTime);

        // Replace with filled cup
        GameObject emptyCup = slotContents[slotIndex];
        if (emptyCup != null)
        {
            // Create new filled cup at the same position
            GameObject filledCup = Instantiate(filledCupPrefab, coffeeSlots[slotIndex].position, Quaternion.identity);

            // Ensure it's active and scaled correctly
            filledCup.SetActive(true);

            // Get the filled cup component and set it as placed
            FilledCup filledCupScript = filledCup.GetComponent<FilledCup>();
            if (filledCupScript != null)
            {
                filledCupScript.SetPlaced();
            }

            // Destroy empty cup
            Destroy(emptyCup);
            slotContents[slotIndex] = filledCup;

            Debug.Log($"Coffee in slot {slotIndex} is ready!");
        }

        brewCoroutines.Remove(slotIndex);
    }

    // Method to cancel brewing if needed
    public void CancelBrewing(int slotIndex)
    {
        if (brewCoroutines.ContainsKey(slotIndex))
        {
            StopCoroutine(brewCoroutines[slotIndex]);
            brewCoroutines.Remove(slotIndex);

            // Optionally handle the cup still in the slot
            Debug.Log($"Brewing canceled in slot {slotIndex}");
        }
    }
}