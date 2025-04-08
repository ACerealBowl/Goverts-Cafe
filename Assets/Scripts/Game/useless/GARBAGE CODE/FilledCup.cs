using UnityEngine;

public class FilledCup : MonoBehaviour
{
    private PlateSystem plateSystem;
    private int cupPosition;
    private bool isBeingHeld = true;

    [SerializeField] private bool debugMode = true;

    private void Start()
    {
        // Find the plate system in the scene
        plateSystem = FindObjectOfType<PlateSystem>();

        if (debugMode)
        {
            Debug.Log($"[FilledCup] Start - Found plateSystem: {plateSystem != null}");
        }

        // Register with the plate system
        if (plateSystem != null)
        {
            plateSystem.RegisterNewItem(gameObject, "Coffee", cupPosition);

            if (debugMode)
            {
                Debug.Log($"[FilledCup] Registered with plate system as Coffee (position: {cupPosition})");
            }
        }
        else if (debugMode)
        {
            Debug.LogError("[FilledCup] PlateSystem not found in the scene!");
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
        Debug.Log($"[FilledCup] Cup position set to {position}");
    }

    // Called when the cup is placed on a plate
    public void SetPlaced()
    {
        isBeingHeld = false;

        if (debugMode)
        {
            Debug.Log("[FilledCup] Cup placed on plate, no longer being held");
        }
    }

    // Called when the cup is picked up from a plate
    public void SetHeld()
    {
        isBeingHeld = true;

        if (debugMode)
        {
            Debug.Log("[FilledCup] Cup picked up from plate, now being held");
        }
    }

    // Check for any potential collider issues
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (debugMode)
        {
            Debug.Log($"[FilledCup] Trigger entered with {collision.gameObject.name}");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (debugMode)
        {
            Debug.Log($"[FilledCup] Collision with {collision.gameObject.name}");
        }
    }
}