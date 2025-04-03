using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pastry : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject pastryPrefab;  // Changed from cake to pastryPrefab
    [SerializeField] private Sprite[] pastrySprites;
    [SerializeField] private string[] pastryTypes = { "Cake", "Donut", "Tiramisu", "Muffin" };
    [SerializeField] private GameObject selectionMenu;
    [SerializeField] private Button[] selectionButtons;
    [SerializeField] private Animator menuAnimator;
    [SerializeField] private PlateSystem plateSystem;

    private bool isMenuOpen = false;
    private GameObject currentPastry = null;  // Track the current pastry being created
    private int currentSpriteIndex = 0;

    private void Start()
    {
        if (selectionMenu != null)
        {
            selectionMenu.SetActive(false);
            CenterMenu();
        }

        for (int i = 0; i < selectionButtons.Length; i++)
        {
            int buttonIndex = i;
            if (selectionButtons[i] != null)
            {
                selectionButtons[i].onClick.AddListener(() => SelectPastry(buttonIndex));
            }
        }
    }

    private void Update()
    {
        // Make the current pastry follow the mouse if it exists
        if (currentPastry != null && currentPastry.activeSelf)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            currentPastry.transform.position = mousePosition;
        }
    }

    private void CenterMenu()
    {
        if (selectionMenu != null)
        {
            RectTransform rectTransform = selectionMenu.GetComponent<RectTransform>();
            rectTransform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        }
    }

    private void OnMouseDown()
    {
        // Only show menu if no pastry is currently being created and menu isn't open
        if (gameObject.activeSelf && currentPastry == null && !isMenuOpen)
        {
            CenterMenu();
            selectionMenu.SetActive(true);
            menuAnimator.SetTrigger("open");
            isMenuOpen = true;
            Debug.Log("Opening pastry menu");
        }
        else
        {
            Debug.Log("Cannot open menu: currentPastry=" + (currentPastry != null ? "exists" : "null") + ", isMenuOpen=" + isMenuOpen);
        }
    }

    private void SelectPastry(int index)
    {
        Debug.Log("SelectPastry called with index: " + index);

        // Create a new instance of the pastry prefab
        if (pastryPrefab != null)
        {
            // Instantiate a new pastry object
            currentPastry = Instantiate(pastryPrefab, transform);
            currentSpriteIndex = index;

            // Set the sprite for the new pastry
            SpriteRenderer pastrySpriteRenderer = currentPastry.GetComponent<SpriteRenderer>();
            if (pastrySpriteRenderer != null && pastrySprites != null && index < pastrySprites.Length)
            {
                pastrySpriteRenderer.sprite = pastrySprites[index];
            }

            // Position at mouse
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            currentPastry.transform.position = mousePosition;
            currentPastry.SetActive(true);

            // Register with the plate system
            if (plateSystem != null && index < pastryTypes.Length)
            {
                plateSystem.RegisterNewItem(currentPastry, pastryTypes[index], index);
                Debug.Log("Registered new item with PlateSystem: " + pastryTypes[index]);
            }
            else
            {
                Debug.LogError("PlateSystem is null or index out of range!");
            }
        }
        else
        {
            Debug.LogError("Pastry prefab is not assigned!");
        }

        selectionMenu.SetActive(false);
        isMenuOpen = false;
    }

    // Called when the pastry is placed on a plate or discarded
    public void ResetPastrySystem()
    {
        // Just clear the reference - don't destroy the object as PlateSystem handles that
        currentPastry = null;
        Debug.Log("Pastry system reset");
    }
}