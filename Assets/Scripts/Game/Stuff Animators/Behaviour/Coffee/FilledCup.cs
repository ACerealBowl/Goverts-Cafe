using UnityEngine;

public class FilledCup : MonoBehaviour
{
    private bool isHeld = false;
    private bool isPlaced = false;

    [SerializeField] private int spriteIndex = 1;
    [SerializeField] private string cupType = "FilledCup";

    // Cached references
    private CameraAnimator cameraAnimator;

    private void Start()
    {
        // Ensure the object is enabled when created
        gameObject.SetActive(true);

        // Set initial scale when spawned
        transform.localScale = new Vector3(0.37f, 0.37f, 0.37f);

        // Cache reference to CameraAnimator
        cameraAnimator = CameraAnimator.Instance;
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

    public void SetHeld()
    {
        isHeld = true;
        isPlaced = false;
        SingularCup.currentlyHeldCup = gameObject;

        // Set scale based on current view when held
        if (cameraAnimator != null && cameraAnimator.IsInCoffeeViewActive)
        {
            transform.localScale = new Vector3(0.50f, 0.50f, 0.50f); // Larger for InCoffeeView
        }
        else
        {
            transform.localScale = new Vector3(0.45f, 0.45f, 0.45f); // Default holding scale
        }

        Debug.Log("Filled cup is now being held");
    }

    public void SetPlaced()
    {
        isHeld = false;
        isPlaced = true;
        SingularCup.currentlyHeldCup = null;

        // Set scale based on current view when placed
        if (cameraAnimator != null)
        {
            if (cameraAnimator.IsInCoffeeViewActive)
            {
                transform.localScale = new Vector3(0.50f, 0.50f, 0.50f); // Larger for InCoffeeView
            }
            else
            {
                transform.localScale = new Vector3(0.37f, 0.37f, 0.37f); // Default placed scale
            }
        }
        else
        {
            transform.localScale = new Vector3(0.37f, 0.37f, 0.37f); // Default placed scale
        }

        Debug.Log("Filled cup has been placed");
    }

    private void DiscardCup()
    {
        if (isHeld)
        {
            isHeld = false;
            SingularCup.currentlyHeldCup = null;
            Debug.Log("Discarded filled cup");
            Destroy(gameObject);
        }
    }

    // For direct click pickup
    private void OnMouseDown()
    {
        // Only allow picking up if not already holding something else 
        // and either in coffee view or not in pastry view
        if (SingularCup.currentlyHeldCup == null)
        {
            if (cameraAnimator != null)
            {
                if (cameraAnimator.IsInCoffeeViewActive || !cameraAnimator.IsPastryViewActive)
                {
                    Debug.Log("Attempting to pick up filled cup");
                    SetHeld();
                }
                else
                {
                    Debug.Log("Cannot pick up filled cup in this view");
                }
            }
            else
            {
                Debug.Log("Attempting to pick up filled cup");
                SetHeld();
            }
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

    public int GetSpriteIndex()
    {
        return spriteIndex;
    }

    // Update the scale based on current view
    public void UpdateScale()
    {
        if (cameraAnimator == null)
        {
            cameraAnimator = CameraAnimator.Instance;
            if (cameraAnimator == null) return;
        }

        if (isHeld)
        {
            if (cameraAnimator.IsInCoffeeViewActive)
            {
                transform.localScale = new Vector3(0.50f, 0.50f, 0.50f); // Larger for InCoffeeView
            }
            else
            {
                transform.localScale = new Vector3(0.45f, 0.45f, 0.45f); // Default holding scale
            }
        }
        else if (isPlaced)
        {
            if (cameraAnimator.IsInCoffeeViewActive)
            {
                transform.localScale = new Vector3(0.50f, 0.50f, 0.50f); // Larger for InCoffeeView
            }
            else
            {
                transform.localScale = new Vector3(0.37f, 0.37f, 0.37f); // Default placed scale
            }
        }
    }

    // Called when camera view changes to update cup's scale
    public void OnViewChanged()
    {
        UpdateScale();
    }

    // Set visibility of the cup
    public void SetVisibility(bool isVisible)
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.enabled = isVisible;
        }
    }

    // Check if cup can be placed in current view
    public bool CanBePlacedInCurrentView()
    {
        if (cameraAnimator == null)
        {
            cameraAnimator = CameraAnimator.Instance;
            if (cameraAnimator == null) return true;
        }

        // FilledCups can be placed in Coffee or InCoffee views or any non-pastry view
        return cameraAnimator.IsInCoffeeViewActive || !cameraAnimator.IsPastryViewActive;
    }

    // Follow the plate when plate position changes
    public void FollowPlatePosition(Vector3 newPosition)
    {
        if (isPlaced)
        {
            transform.position = newPosition;
        }
    }
}