using UnityEngine;

public class SingularCup : MonoBehaviour
{
    private CleanCups cleanCups;
    private CoffeeMachine coffeeMachine;

    [SerializeField] private bool debugMode = true;
    private Pastry pastrySystem; // Reference to check if pastry system is active

    private void Awake()
    {
        cleanCups = FindObjectOfType<CleanCups>();
        coffeeMachine = FindObjectOfType<CoffeeMachine>();
        pastrySystem = FindObjectOfType<Pastry>(); // Find the pastry system

        if (debugMode)
        {
            Debug.Log($"[SingularCup] Initialized. Found coffeeMachine: {coffeeMachine != null}, " +
                     $"cleanCups: {cleanCups != null}, pastrySystem: {pastrySystem != null}");
        }
    }

    private void OnMouseDown()
    {
        if (debugMode)
        {
            Debug.Log($"[SingularCup] OnMouseDown triggered at position {transform.position}");

            // Check if pastry system is active
            if (pastrySystem != null)
            {
                Debug.Log($"[SingularCup] Pastry system exists. Is pastry system active? Check its state here.");
            }
        }

        bool isOverCoffee = IsOverCoffeeMachine();

        if (debugMode)
        {
            Debug.Log($"[SingularCup] Is over coffee machine: {isOverCoffee}");
        }

        if (isOverCoffee)
        {
            if (coffeeMachine != null)
            {
                bool canAccept = coffeeMachine.CanAcceptCup();

                if (debugMode)
                {
                    Debug.Log($"[SingularCup] Coffee machine can accept cup: {canAccept}");
                }

                if (canAccept)
                {
                    coffeeMachine.AddCup(transform.position);
                    if (cleanCups != null)
                    {
                        cleanCups.ProcessEmptyCup();
                        if (debugMode)
                        {
                            Debug.Log("[SingularCup] Cup processed by cleanCups system");
                        }
                    }
                    else if (debugMode)
                    {
                        Debug.LogError("[SingularCup] cleanCups reference is null!");
                    }
                }
                else if (debugMode)
                {
                    Debug.Log("[SingularCup] Coffee machine cannot accept cup at this time");
                }
            }
            else if (debugMode)
            {
                Debug.LogError("[SingularCup] coffeeMachine reference is null!");
            }
        }
    }

    private bool IsOverCoffeeMachine()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.zero);

        if (debugMode)
        {
            Debug.Log($"[SingularCup] Raycast detected {hits.Length} collisions at position {transform.position}");

            foreach (RaycastHit2D hit in hits)
            {
                Debug.Log($"[SingularCup] Raycast hit: {hit.collider.name}, Tag: {hit.collider.tag}");
            }
        }

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("CoffeeMachine"))
            {
                return true;
            }
        }
        return false;
    }

    // Helper debug method to visualize raycasts
    private void OnDrawGizmos()
    {
        if (debugMode)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 0.1f);
            Gizmos.DrawWireSphere(transform.position, 0.2f);
        }
    }
}