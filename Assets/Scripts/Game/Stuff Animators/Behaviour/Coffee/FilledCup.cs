using UnityEngine;
public class FilledCup : MonoBehaviour
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
            if (coffeeMachine != null && coffeeMachine.CanRemoveCup())
            {
                coffeeMachine.RemoveCup();
                cleanCups.SpawnFilledCup(transform.position);
            }
        }
    }

    private bool IsOverCoffeeMachine()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.zero);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.name.ToLower().Contains("coffeemachine"))
            {
                return true;
            }
        }
        return false;
    }
}