using System.Collections.Generic;
using UnityEngine;

public class PlateSystem : MonoBehaviour
{
    [SerializeField] private Transform[] plates;
    [SerializeField] private float plateRadius = 1.0f;
    [SerializeField] private Pastry pastrySystem;

    private GameObject currentPickedItem = null;
    private string currentItemType = "";
    private int currentSpriteIndex = -1;

    private void Start()
    {
        InitializePlateIdentifiers();
        FindPastrySystemIfNull();
    }

    private void InitializePlateIdentifiers()
    {
        for (int i = 0; i < plates.Length; i++)
        {
            PlateIdentifier identifier = plates[i].GetComponent<PlateIdentifier>();
            if (identifier == null)
            {
                identifier = plates[i].gameObject.AddComponent<PlateIdentifier>();
            }

            if (identifier.GetPlateIdAsInt() <= 0)
            {
                identifier.SetPlateId((i + 1).ToString());
                Debug.Log($"Set plate {i} ID to {i + 1}");
            }
        }
    }

    private bool IsFilledCupItem(GameObject item)
    {
        foreach (Transform plate in plates)
        {
            var itemsOnPlate = GameManager.Instance.GetItemsOnPlate(plate.gameObject);
            foreach (var itemData in itemsOnPlate)
            {
                if (itemData.itemObject == item && itemData.itemType == "FilledCup")
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void FindPastrySystemIfNull()
    {
        if (pastrySystem == null)
        {
            pastrySystem = FindObjectOfType<Pastry>();
        }
    }

    private void Update()
    {
        HandleRightClick();
        HandleLeftClick();
        UpdateHeldItemPosition();
    }

    private void HandleRightClick()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (currentPickedItem != null)
            {
                DiscardCurrentItem();
                return;
            }

            Vector3 mousePosition = GetMouseWorldPosition();
            Transform plateUnderMouse = GetPlateUnderMouse(mousePosition);

            if (plateUnderMouse != null)
            {
                PlateController plateController = plateUnderMouse.GetComponent<PlateController>();
                plateController?.HandlePlateRightClick(mousePosition);
            }
        }
    }

    private void HandleLeftClick()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Vector3 mousePosition = GetMouseWorldPosition();

        if (currentPickedItem != null)
        {
            HandleItemPlacement(mousePosition);
            return;
        }

        TryPickUpItem(mousePosition);
    }

    private void HandleItemPlacement(Vector3 mousePosition)
    {
        Transform targetPlate = GetPlateUnderMouse(mousePosition);
        if (targetPlate != null)
        {
            // Prevent placing pastries in non-pastry views
            if (IsPastryType(currentItemType) && !CameraAnimator.Instance.IsPastryViewActive)
            {
                DiscardCurrentItem();
                return;
            }

            // Check for FilledCup placement in views
            if (currentItemType == "FilledCup" && !CanPlaceFilledCupInCurrentView())
            {
                Debug.Log("Cannot place filled cup in this view");
                DiscardCurrentItem();
                return;
            }

            PlaceItemOnPlate(targetPlate.gameObject, mousePosition);
        }
    }

    private bool CanPlaceFilledCupInCurrentView()
    {
        // FilledCups can be placed in Coffee or InCoffee views
        if (CameraAnimator.Instance != null)
        {
            return CameraAnimator.Instance.IsInCoffeeViewActive ||
                   !CameraAnimator.Instance.IsPastryViewActive; // Allow in any non-pastry view
        }
        return true; // Default to allowing placement if CameraAnimator can't be found
    }

    private void TryPickUpItem(Vector3 mousePosition)
    {
        GameObject itemToPickUp = GetItemUnderMouse(mousePosition);
        if (itemToPickUp != null)
        {
            // Prevent picking up pastries in non-pastry views
            if (IsPastryItem(itemToPickUp) && !CameraAnimator.Instance.IsPastryViewActive)
            {
                Debug.Log("Can't interact with pastries in this view");
                return;
            }

            // Prevent picking up filled cups in certain views
            if (IsFilledCupItem(itemToPickUp) && !CanPickUpFilledCupInCurrentView())
            {
                Debug.Log("Can't pick up filled cup in this view");
                return;
            }

            PickUpExistingItem(itemToPickUp, mousePosition);
        }
    }

    private bool CanPickUpFilledCupInCurrentView()
    {
        // FilledCups can be picked up in Coffee or InCoffee views or any non-pastry view
        if (CameraAnimator.Instance != null)
        {
            return CameraAnimator.Instance.IsInCoffeeViewActive ||
                   !CameraAnimator.Instance.IsPastryViewActive;
        }
        return true; // Default to allowing pickup if CameraAnimator can't be found
    }

    private bool IsPastryItem(GameObject item)
    {
        foreach (Transform plate in plates)
        {
            var itemsOnPlate = GameManager.Instance.GetItemsOnPlate(plate.gameObject);
            foreach (var itemData in itemsOnPlate)
            {
                if (itemData.itemObject == item && IsPastryType(itemData.itemType))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void UpdateHeldItemPosition()
    {
        if (currentPickedItem != null)
        {
            currentPickedItem.transform.position = GetMouseWorldPosition();
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        return mousePos;
    }

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

    private GameObject GetItemUnderMouse(Vector3 mousePosition)
    {
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

    private void PickUpExistingItem(GameObject item, Vector3 position)
    {
        foreach (Transform plate in plates)
        {
            List<GameManager.PlacedItemData> itemsOnPlate =
                GameManager.Instance.GetItemsOnPlate(plate.gameObject);

            foreach (GameManager.PlacedItemData itemData in itemsOnPlate)
            {
                if (itemData.itemObject == item)
                {
                    currentPickedItem = item;
                    currentItemType = itemData.itemType;
                    currentSpriteIndex = itemData.spriteIndex;

                    GameManager.Instance.RemoveItemFromPlate(item, plate.gameObject);

                    FilledCup filledCup = item.GetComponent<FilledCup>();
                    if (filledCup != null)
                    {
                        filledCup.SetHeld();

                        // Apply appropriate scale when picked up based on current view
                        if (CameraAnimator.Instance != null)
                        {
                            if (CameraAnimator.Instance.IsInCoffeeViewActive)
                            {
                                // Already handled by FilledCup.SetHeld()
                            }
                        }
                    }

                    Debug.Log($"Picked up {currentItemType} from plate {plate.GetComponent<PlateIdentifier>().GetPlateId()}");
                    return;
                }
            }
        }
    }

    private void PlaceItemOnPlate(GameObject plate, Vector3 position)
    {
        currentPickedItem.transform.position = position;

        PlateIdentifier plateIdentifier = plate.GetComponent<PlateIdentifier>();
        if (plateIdentifier != null)
        {
            plateIdentifier.RegisterItemPosition(currentPickedItem);
        }

        GameManager.Instance.AddItemToPlate(currentPickedItem, plate, currentItemType, currentSpriteIndex);

        FilledCup filledCup = currentPickedItem.GetComponent<FilledCup>();
        if (filledCup != null)
        {
            filledCup.SetPlaced();

            // Apply appropriate scale when placed based on current view
            if (CameraAnimator.Instance != null)
            {
                // The scale is already handled by FilledCup.SetPlaced() and CameraAnimator.ScaleAllFilledCups
            }
        }

        if (IsPastryType(currentItemType) && pastrySystem != null)
        {
            pastrySystem.ResetPastrySystem();
        }

        Debug.Log($"Placed {currentItemType} on plate {plateIdentifier.GetPlateId()}");

        currentPickedItem = null;
        currentItemType = "";
        currentSpriteIndex = -1;
    }

    private void DiscardCurrentItem()
    {
        if (currentPickedItem != null)
        {
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

    private bool IsPastryType(string itemType)
    {
        return itemType == "Cake" || itemType == "Donut" ||
               itemType == "Tiramisu" || itemType == "Muffin";
    }

    public void RegisterNewItem(GameObject item, string itemType, int spriteIndex)
    {
        if (currentPickedItem != null)
        {
            DiscardCurrentItem();
        }

        currentPickedItem = item;
        currentItemType = itemType;
        currentSpriteIndex = spriteIndex;

        Debug.Log($"Registered new {itemType} (sprite: {spriteIndex})");
    }
}