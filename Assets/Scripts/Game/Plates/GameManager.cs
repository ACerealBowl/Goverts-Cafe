using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Dictionary to track what's on each plate - plate ID as key
    private Dictionary<string, List<PlacedItemData>> plateContents = new Dictionary<string, List<PlacedItemData>>();

    [System.Serializable]
    public class PlacedItemData
    {
        public string itemType; // e.g., "cake", "coffee", "donut", "tiramisu", "muffin"
        public int spriteIndex; // Which sprite variant was used (for pastries)
        public Vector3 position;
        public GameObject itemObject; // Reference to the actual GameObject

        public PlacedItemData(string type, int spriteIndex, Vector3 pos, GameObject obj)
        {
            itemType = type;
            this.spriteIndex = spriteIndex;
            position = pos;
            itemObject = obj;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Add an item to a plate and track it
    public void AddItemToPlate(GameObject item, GameObject plate, string itemType, int spriteIndex)
    {
        // Get the plate identifier component
        PlateIdentifier plateIdentifier = plate.GetComponent<PlateIdentifier>();
        if (plateIdentifier == null)
        {
            Debug.LogError("Plate is missing PlateIdentifier component!");
            return;
        }

        string plateId = plateIdentifier.GetPlateId();

        // Initialize the list for this plate if it doesn't exist
        if (!plateContents.ContainsKey(plateId))
        {
            plateContents[plateId] = new List<PlacedItemData>();
        }

        // Create and store the item data
        PlacedItemData data = new PlacedItemData(itemType, spriteIndex, item.transform.position, item);
        plateContents[plateId].Add(data);

        Debug.Log($"Added {itemType} (sprite: {spriteIndex}) to plate {plateId}");
    }

    // Remove an item from a plate
    public void RemoveItemFromPlate(GameObject item, GameObject plate)
    {
        // Get the plate identifier component
        PlateIdentifier plateIdentifier = plate.GetComponent<PlateIdentifier>();
        if (plateIdentifier == null)
        {
            Debug.LogError("Plate is missing PlateIdentifier component!");
            return;
        }

        string plateId = plateIdentifier.GetPlateId();

        if (plateContents.ContainsKey(plateId))
        {
            // Find the exact item in our data
            PlacedItemData itemToRemove = null;
            foreach (var placedItem in plateContents[plateId])
            {
                if (placedItem.itemObject == item)
                {
                    itemToRemove = placedItem;
                    break;
                }
            }

            if (itemToRemove != null)
            {
                plateContents[plateId].Remove(itemToRemove);
                Debug.Log($"Removed {itemToRemove.itemType} from plate {plateId}");
            }
        }
    }

    // Get all items on a specific plate
    public List<PlacedItemData> GetItemsOnPlate(GameObject plate)
    {
        // Get the plate identifier component
        PlateIdentifier plateIdentifier = plate.GetComponent<PlateIdentifier>();
        if (plateIdentifier == null)
        {
            Debug.LogError("Plate is missing PlateIdentifier component!");
            return new List<PlacedItemData>();
        }

        string plateId = plateIdentifier.GetPlateId();

        if (plateContents.ContainsKey(plateId))
        {
            return plateContents[plateId];
        }
        return new List<PlacedItemData>();
    }

    // Get all plates and their contents
    public Dictionary<string, List<PlacedItemData>> GetAllPlateContents()
    {
        return plateContents;
    }

    // Check if a specific plate has the given item type
    public bool PlateHasItemType(GameObject plate, string itemType)
    {
        // Get the plate identifier component
        PlateIdentifier plateIdentifier = plate.GetComponent<PlateIdentifier>();
        if (plateIdentifier == null)
        {
            Debug.LogError("Plate is missing PlateIdentifier component!");
            return false;
        }

        string plateId = plateIdentifier.GetPlateId();

        if (plateContents.ContainsKey(plateId))
        {
            foreach (var item in plateContents[plateId])
            {
                if (item.itemType == itemType)
                {
                    return true;
                }
            }
        }
        return false;
    }
}