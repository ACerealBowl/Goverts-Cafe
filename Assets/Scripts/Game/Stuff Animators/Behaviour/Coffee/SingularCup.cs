using UnityEngine;

public class SingularCup : MonoBehaviour
{
    private CleanCups cleanCups;
    private CoffeeMachine coffeeMachine;

    private void Awake()
    {
        cleanCups = FindObjectOfType<CleanCups>();
        coffeeMachine = FindObjectOfType<CoffeeMachine>();
    }

    private void OnMouseDown()
    {
        if (IsOverCoffeeMachine())
        {
            if (coffeeMachine != null && coffeeMachine.CanAcceptCup())
            {
                coffeeMachine.AddCup(transform.position);  // Now passing the cup's position
                if (cleanCups != null)
                {
                    cleanCups.ProcessEmptyCup();
                }
            }
        }
    }

    private bool IsOverCoffeeMachine()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.zero);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("CoffeeMachine"))  // Changed to use tag instead of name
            {
                return true;
            }
        }
        return false;
    }
}