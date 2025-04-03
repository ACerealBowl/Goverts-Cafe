using System.Collections.Generic;
using UnityEngine;

public class PlateSystem : MonoBehaviour
{
    [SerializeField] private Transform[] plates;
    [SerializeField] private float plateRadius = 1.0f; // Adjust based on your plate size
    [SerializeField] private Pastry pastrySystem; // Reference to the Pastry script

    private GameObject currentPickedItem = null;
    private string currentItemType = "";
    private int currentSpriteIndex = -1;

    private void Start()
    {
        // Find the pastry system if not assigned
        if (pastrySystem == null)
        {
            pastrySystem = FindObjectOfType<Pastry>();
        }

        // Ensure all plates have PlateIdentifier
        foreach (Transform plate in plates)
        {
            if (plate.GetComponent<PlateIdentifier>() == null)
            {
                plate.gameObject.AddComponent<PlateIdentifier>();
            }
        }
    }

    private void Update()
    {
        // Right-click to discard the current item or remove item from plate
        if (Input.GetMouseButtonDown(1))
        {
            if (currentPickedItem != null)
            {
                // Discard item being held
                DiscardCurrentItem();
                return;
            }
            else
            {
                // Try to remove an item from a plate via right-click
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0f;

                // Check if we clicked on a plate
                Transform plateUnderMouse = GetPlateUnderMouse(mousePosition);
                if (plateUnderMouse != null)
                {
                    // Forward to plate controller
                    PlateController plateController = plateUnderMouse.GetComponent<PlateController>();
                    if (plateController != null)
                    {
                        plateController.HandlePlateRightClick(mousePosition);
                    }
                }
            }
        }

        // Left-click to place or pick up an item
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;

            // If we have an item picked up, try to place it
            if (currentPickedItem != null)
            {
                Transform targetPlate = GetPlateUnderMouse(mousePosition);
                if (targetPlate != null)
                {
                    PlaceItemOnPlate(targetPlate.gameObject, mousePosition);
                }
                return;
            }

            // If we don't have an item, try to pick one up from a plate
            GameObject itemToPickUp = GetItemUnderMouse(mousePosition);
            if (itemToPickUp != null)
            {
                PickUpExistingItem(itemToPickUp, mousePosition);
            }
        }

        // Update position of picked item
        if (currentPickedItem != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            currentPickedItem.transform.position = mousePos;
        }
    }

    // Find a plate under the mouse position
    public Transform GetPlateUnderMouse(Vector3 mousePosition)
    {
        foreach (Transform plate in plates)
        {
            if (Vector2.Distance(new Vector2(mousePosition.x, mousePosition.y),
                               new Vector2(plate.position.x, plate.position.y)) <= plateRadius)
            {
                return plate;
            }
        }
        return null;
    }

    // Find an already placed item under the mouse
    private GameObject GetItemUnderMouse(Vector3 mousePosition)
    {
        // Check all plates for items
        foreach (Transform plate in plates)
        {
            List<GameManager.PlacedItemData> itemsOnPlate =
                GameManager.Instance.GetItemsOnPlate(plate.gameObject);

            foreach (GameManager.PlacedItemData itemData in itemsOnPlate)
            {
                if (Vector2.Distance(new Vector2(mousePosition.x, mousePosition.y),
                                    new Vector2(itemData.position.x, itemData.position.y)) <= 0.5f)
                {
                    return itemData.itemObject;
                }
            }
        }
        return null;
    }

    // Pick up an item that was already placed on a plate
    private void PickUpExistingItem(GameObject item, Vector3 position)
    {
        // Find which plate this item belongs to
        foreach (Transform plate in plates)
        {
            List<GameManager.PlacedItemData> itemsOnPlate =
                GameManager.Instance.GetItemsOnPlate(plate.gameObject);

            foreach (GameManager.PlacedItemData itemData in itemsOnPlate)
            {
                if (itemData.itemObject == item)
                {
                    // Store the item data
                    currentPickedItem = item;
                    currentItemType = itemData.itemType;
                    currentSpriteIndex = itemData.spriteIndex;

                    // Remove from plate tracking
                    GameManager.Instance.RemoveItemFromPlate(item, plate.gameObject);

                    // If it's a coffee cup, notify it
                    FilledCup filledCup = item.GetComponent<FilledCup>();
                    if (filledCup != null)
                    {
                        filledCup.SetHeld();
                    }

                    Debug.Log($"Picked up {currentItemType} from plate");
                    return;
                }
            }
        }
    }

    // Place the current item on a plate
    private void PlaceItemOnPlate(GameObject plate, Vector3 position)
    {
        if (currentPickedItem != null)
        {
            // Set the final position
            currentPickedItem.transform.position = position;

            // Add to GameManager tracking
            GameManager.Instance.AddItemToPlate(currentPickedItem, plate, currentItemType, currentSpriteIndex);

            // Notify coffee cup if applicable
            FilledCup filledCup = currentPickedItem.GetComponent<FilledCup>();
            if (filledCup != null)
            {
                filledCup.SetPlaced();
            }

            // Reset the pastry system if this was a pastry item
            if (IsPastryType(currentItemType) && pastrySystem != null)
            {
                pastrySystem.ResetPastrySystem();
            }

            // Reset current item
            currentPickedItem = null;
            currentItemType = "";
            currentSpriteIndex = -1;

            Debug.Log("Placed item on plate");
        }
    }

    // Discard the current item being held
    private void DiscardCurrentItem()
    {
        if (currentPickedItem != null)
        {
            // Reset the pastry system if this was a pastry item
            if (IsPastryType(currentItemType) && pastrySystem != null)
            {
                pastrySystem.ResetPastrySystem();
            }

            Destroy(currentPickedItem);
            currentPickedItem = null;
            currentItemType = "";
            currentSpriteIndex = -1;
            Debug.Log("Discarded item");
        }
    }

    // Check if an item type is a pastry
    private bool IsPastryType(string itemType)
    {
        return itemType == "Cake" || itemType == "Donut" ||
               itemType == "Tiramisu" || itemType == "Muffin";
    }

    // This method should be called by other scripts (like Pastry) when an item is created
    public void RegisterNewItem(GameObject item, string itemType, int spriteIndex)
    {
        // If we're already holding something, discard it
        if (currentPickedItem != null)
        {
            DiscardCurrentItem();
        }

        // Set the new item
        currentPickedItem = item;
        currentItemType = itemType;
        currentSpriteIndex = spriteIndex;

        Debug.Log($"Registered new {itemType} (sprite: {spriteIndex})");
    }
}