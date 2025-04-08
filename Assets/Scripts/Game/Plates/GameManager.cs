using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Dictionary<string, List<PlacedItemData>> plateContents = new Dictionary<string, List<PlacedItemData>>();

    [System.Serializable]
    public class PlacedItemData
    {
        public string itemType;
        public int spriteIndex;
        public Vector3 position;
        public GameObject itemObject;
        public Vector3 localPosition;

        public PlacedItemData(string type, int spriteIndex, Vector3 pos, GameObject obj, Vector3 localPos)
        {
            itemType = type;
            this.spriteIndex = spriteIndex;
            position = pos;
            itemObject = obj;
            localPosition = localPos;
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

    public void AddItemToPlate(GameObject item, GameObject plate, string itemType, int spriteIndex)
    {
        PlateIdentifier plateIdentifier = plate.GetComponent<PlateIdentifier>();
        if (plateIdentifier == null)
        {
            Debug.LogError("Plate is missing PlateIdentifier component!");
            return;
        }

        string plateId = plateIdentifier.GetPlateId();

        if (!plateContents.ContainsKey(plateId))
        {
            plateContents[plateId] = new List<PlacedItemData>();
        }

        Vector3 localPosition = item.transform.position - plate.transform.position;
        PlacedItemData data = new PlacedItemData(itemType, spriteIndex, item.transform.position, item, localPosition);
        plateContents[plateId].Add(data);

        Renderer itemRenderer = item.GetComponent<Renderer>();
        if (itemRenderer != null && !plateIdentifier.IsVisible())
        {
            itemRenderer.enabled = false;
        }

        Debug.Log($"Added {itemType} (sprite: {spriteIndex}) to plate {plateId}");
    }

    public void RemoveItemFromPlate(GameObject item, GameObject plate)
    {
        PlateIdentifier plateIdentifier = plate.GetComponent<PlateIdentifier>();
        if (plateIdentifier == null)
        {
            Debug.LogError("Plate is missing PlateIdentifier component!");
            return;
        }

        string plateId = plateIdentifier.GetPlateId();

        if (plateContents.ContainsKey(plateId))
        {
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

    public List<PlacedItemData> GetItemsOnPlate(GameObject plate)
    {
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

    public Dictionary<string, List<PlacedItemData>> GetAllPlateContents()
    {
        return plateContents;
    }

    public bool PlateHasItemType(GameObject plate, string itemType)
    {
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

    public void UpdateAllPlateItemPositions()
    {
        PlateIdentifier[] allPlates = FindObjectsOfType<PlateIdentifier>();
        foreach (var plateIdentifier in allPlates)
        {
            plateIdentifier.UpdateItemPositions();
        }
    }

    public void SetAllPlatesVisibility(bool visible)
    {
        PlateIdentifier[] allPlates = FindObjectsOfType<PlateIdentifier>();
        foreach (var plateIdentifier in allPlates)
        {
            plateIdentifier.SetVisibility(visible);
        }
        Debug.Log($"Set visibility of all plates and pastries to {visible}");
    }

    public void SetPlateVisibility(int plateId, bool visible)
    {
        PlateIdentifier[] allPlates = FindObjectsOfType<PlateIdentifier>();
        foreach (var plateIdentifier in allPlates)
        {
            if (plateIdentifier.GetPlateIdAsInt() == plateId)
            {
                plateIdentifier.SetVisibility(visible);
                return;
            }
        }
        Debug.LogWarning($"Plate with ID {plateId} not found for visibility change");
    }
}