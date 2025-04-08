using UnityEngine;

public class FilledCup : MonoBehaviour
{
    private PlateSystem plateSystem;
    private int cupPosition;
    private bool isBeingHeld = true;

    private void Start()
    {
        // Find the plate system in the scene
        plateSystem = FindObjectOfType<PlateSystem>();

        // Register with the plate system
        if (plateSystem != null)
        {
            plateSystem.RegisterNewItem(gameObject, "Coffee", cupPosition);
        }
    }

    private void Update()
    {
        // Only update position if the object is active and being held
        if (gameObject.activeSelf && isBeingHeld)
        {
            FollowMouse();
        }
    }

    private void FollowMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -Camera.main.transform.position.z; // Set depth to match main camera's view
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 0; // Keep the object on the same plane

        transform.position = worldPosition;
    }

    public void SetCupPosition(int position)
    {
        cupPosition = position;
        Debug.Log($"Cup position set to {position}");
    }

    // Called when the cup is placed on a plate
    public void SetPlaced()
    {
        isBeingHeld = false;
    }

    // Called when the cup is picked up from a plate
    public void SetHeld()
    {
        isBeingHeld = true;
    }
}