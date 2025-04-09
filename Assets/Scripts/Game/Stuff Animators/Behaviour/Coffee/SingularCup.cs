using UnityEngine;
using System.Collections;

public class SingularCup : MonoBehaviour
{
    public static GameObject currentlyHeldCup;

    [SerializeField] private string cupType = "EmptyCup";
    [SerializeField] private int spriteIndex = 0;

    private bool isHeld = false;
    private bool isPlaced = false;

    private void Start()
    {
        // Initially not visible until picked up
        if (!isHeld && !isPlaced)
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (isHeld)
        {
            // Follow mouse position
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            transform.position = mousePos;

            // Handle right-click to discard
            if (Input.GetMouseButtonDown(1))
            {
                DiscardCup();
            }
        }
    }

    public void PickUp()
    {
        if (currentlyHeldCup != null)
        {
            Debug.LogWarning("Already holding a cup, can't pick up another!");
            return;
        }

        isHeld = true;
        isPlaced = false;
        currentlyHeldCup = gameObject;
        gameObject.SetActive(true);

        // Set scale to 1 when held (empty cup)
        transform.localScale = Vector3.one;

        Debug.Log($"Picked up {cupType}");
    }

    public void PlaceDown()
    {
        isHeld = false;
        isPlaced = true;
        currentlyHeldCup = null;

        // Set scale to 0.8 when placed (empty cup)
        transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

        Debug.Log($"Placed down {cupType}");
    }

    public void DiscardCup()
    {
        if (isHeld)
        {
            isHeld = false;
            currentlyHeldCup = null;
            Debug.Log($"Discarded {cupType}");
            Destroy(gameObject);
        }
    }

    public bool IsHeld()
    {
        return isHeld;
    }

    public bool IsPlaced()
    {
        return isPlaced;
    }

    public string GetCupType()
    {
        return cupType;
    }

    public void SetCupType(string newType)
    {
        cupType = newType;
    }

    public int GetSpriteIndex()
    {
        return spriteIndex;
    }

    public void SetSpriteIndex(int index)
    {
        spriteIndex = index;
    }
}