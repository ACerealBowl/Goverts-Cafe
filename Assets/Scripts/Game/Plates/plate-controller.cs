using UnityEngine;

public class PlateController : MonoBehaviour
{
    [SerializeField] private PlateSystem plateSystem;
    private PlateIdentifier plateIdentifier;

    private void Start()
    {
        if (plateSystem == null)
        {
            plateSystem = FindObjectOfType<PlateSystem>();
            if (plateSystem == null)
            {
                Debug.LogError("PlateSystem not found in the scene!");
            }
        }

        plateIdentifier = GetComponent<PlateIdentifier>();
        if (plateIdentifier == null)
        {
            plateIdentifier = gameObject.AddComponent<PlateIdentifier>();
        }
    }

    public void HandlePlateRightClick(Vector3 mousePosition)
    {
        var itemsOnPlate = GameManager.Instance.GetItemsOnPlate(gameObject);

        foreach (var itemData in itemsOnPlate)
        {
            if (Vector2.Distance(new Vector2(mousePosition.x, mousePosition.y),
                                new Vector2(itemData.position.x, itemData.position.y)) <= 0.5f)
            {
                GameManager.Instance.RemoveItemFromPlate(itemData.itemObject, gameObject);
                Destroy(itemData.itemObject);
                Debug.Log($"Removed {itemData.itemType} from plate via right-click");
                return;
            }
        }
    }
}