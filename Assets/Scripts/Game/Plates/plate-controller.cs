using UnityEngine;

public class PlateController : MonoBehaviour
{
    [SerializeField] private PlateSystem plateSystem;
    private PlateIdentifier plateIdentifier;

    private void Start()
    {
        // Find the plate system if not assigned
        if (plateSystem == null)
        {
            plateSystem = FindObjectOfType<PlateSystem>();
            if (plateSystem == null)
            {
                Debug.LogError("PlateSystem not found in the scene!");
            }
        }

        // Ensure plate has an identifier
        plateIdentifier = GetComponent<PlateIdentifier>();
        if (plateIdentifier == null)
        {
            plateIdentifier = gameObject.AddComponent<PlateIdentifier>();
        }
    }

    private void OnMouseDown()
    {
        // Handle left click on plate - forward the click to the PlateSystem
        // This allows the plate to be a target for item placement
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        // No need to call anything here as the PlateSystem handles the placement
        // in its Update method when detecting clicks
    }

    // Make this plate a valid target for the PlateSystem's GetPlateUnderMouse method
    public Transform GetTransform()
    {
        return transform;
    }

    public void HandlePlateRightClick(Vector3 mousePosition)
    {
        // Get items on this plate from GameManager
        var itemsOnPlate = GameManager.Instance.GetItemsOnPlate(gameObject);

        // Find item under mouse position
        foreach (var itemData in itemsOnPlate)
        {
            if (Vector2.Distance(new Vector2(mousePosition.x, mousePosition.y),
                                new Vector2(itemData.position.x, itemData.position.y)) <= 0.5f)
            {
                // Remove item from plate tracking
                GameManager.Instance.RemoveItemFromPlate(itemData.itemObject, gameObject);

                // Destroy the actual object
                Destroy(itemData.itemObject);

                Debug.Log($"Removed {itemData.itemType} from plate via right-click");
                return;
            }
        }
    }
}