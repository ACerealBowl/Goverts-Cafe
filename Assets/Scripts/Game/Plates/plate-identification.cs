using UnityEngine;
using System;
using System.Collections.Generic;

public class PlateIdentifier : MonoBehaviour
{
    [SerializeField] private int plateId = 0;
    private Dictionary<GameObject, Vector3> itemLocalPositions = new Dictionary<GameObject, Vector3>();
    private bool isVisible = true;

    private void Awake()
    {
        if (plateId <= 0)
        {
            PlateIdentifier[] allPlates = FindObjectsOfType<PlateIdentifier>();
            int maxId = 0;
            foreach (var plate in allPlates)
            {
                if (plate.plateId > maxId)
                    maxId = plate.plateId;
            }
            plateId = maxId + 1;
            Debug.Log($"Auto-assigned plate ID: {plateId}");
        }
    }

    public string GetPlateId()
    {
        return plateId.ToString();
    }

    public int GetPlateIdAsInt()
    {
        return plateId;
    }

    public void SetPlateId(string newId)
    {
        if (int.TryParse(newId, out int newIdInt))
        {
            plateId = newIdInt;
        }
        else
        {
            Debug.LogError($"Failed to set plate ID: {newId} is not a valid integer");
        }
    }

    public void RegisterItemPosition(GameObject item)
    {
        Vector3 localPos = item.transform.position - transform.position;
        itemLocalPositions[item] = localPos;
    }

    public Vector3 GetItemLocalPosition(GameObject item)
    {
        if (itemLocalPositions.ContainsKey(item))
        {
            return itemLocalPositions[item];
        }
        return Vector3.zero;
    }

    public void UpdateItemPositions()
    {
        var itemsOnPlate = GameManager.Instance.GetItemsOnPlate(gameObject);

        foreach (var itemData in itemsOnPlate)
        {
            if (itemLocalPositions.ContainsKey(itemData.itemObject))
            {
                itemData.itemObject.transform.position = transform.position + itemLocalPositions[itemData.itemObject];
            }
        }
    }

    public void SetVisibility(bool visible)
    {
        isVisible = visible;

        Renderer plateRenderer = GetComponent<Renderer>();
        if (plateRenderer != null)
        {
            plateRenderer.enabled = visible;
        }

        var itemsOnPlate = GameManager.Instance.GetItemsOnPlate(gameObject);
        foreach (var itemData in itemsOnPlate)
        {
            if (itemData.itemObject != null)
            {
                Renderer itemRenderer = itemData.itemObject.GetComponent<Renderer>();
                if (itemRenderer != null)
                {
                    itemRenderer.enabled = visible;
                }
            }
        }

        Debug.Log($"Plate {plateId} visibility set to {visible}");
    }

    public bool IsVisible()
    {
        return isVisible;
    }

    private void LateUpdate()
    {
        UpdateItemPositions();
    }
}