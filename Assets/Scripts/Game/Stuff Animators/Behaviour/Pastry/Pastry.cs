using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pastry : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject pastryPrefab;
    [SerializeField] private Sprite[] pastrySprites;
    [SerializeField] private string[] pastryTypes = { "Cake", "Donut", "Tiramisu", "Muffin" };
    [SerializeField] private GameObject selectionMenu;
    [SerializeField] private Button[] selectionButtons;
    [SerializeField] private Animator menuAnimator;
    [SerializeField] private PlateSystem plateSystem;

    [SerializeField] private bool isVisible = true;

    private bool isMenuOpen = false;
    private GameObject currentPastry = null;
    private int currentSpriteIndex = 0;
    private List<GameObject> activeUnplacedPastries = new List<GameObject>();

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
        if (!isVisible || !CameraAnimator.Instance.IsPastryViewActive || isMenuOpen || currentPastry != null)
        {
            return;
        }

        CenterMenu();
        selectionMenu.SetActive(true);
        menuAnimator.SetTrigger("open");
        isMenuOpen = true;
        Debug.Log("Opening pastry menu");
    }

    private void SelectPastry(int index)
    {
        Debug.Log("SelectPastry called with index: " + index);

        if (pastryPrefab != null)
        {
            currentPastry = Instantiate(pastryPrefab, transform);
            currentSpriteIndex = index;
            activeUnplacedPastries.Add(currentPastry);

            SpriteRenderer pastrySpriteRenderer = currentPastry.GetComponent<SpriteRenderer>();
            if (pastrySpriteRenderer != null && pastrySprites != null && index < pastrySprites.Length)
            {
                pastrySpriteRenderer.sprite = pastrySprites[index];
            }

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            currentPastry.transform.position = mousePosition;
            currentPastry.SetActive(true);

            if (plateSystem != null && index < pastryTypes.Length)
            {
                plateSystem.RegisterNewItem(currentPastry, pastryTypes[index], index);
                Debug.Log("Registered new item with PlateSystem: " + pastryTypes[index]);
            }
        }

        selectionMenu.SetActive(false);
        isMenuOpen = false;
    }

    public void ResetPastrySystem()
    {
        if (currentPastry != null)
        {
            activeUnplacedPastries.Remove(currentPastry);
        }
        currentPastry = null;
        Debug.Log("Pastry system reset");
    }

    public void SetVisibility(bool visible)
    {
        isVisible = visible;

        SpriteRenderer dispenserRenderer = GetComponent<SpriteRenderer>();
        if (dispenserRenderer != null)
        {
            dispenserRenderer.enabled = visible;
        }

        foreach (var pastry in activeUnplacedPastries)
        {
            if (pastry != null)
            {
                SpriteRenderer pastryRenderer = pastry.GetComponent<SpriteRenderer>();
                if (pastryRenderer != null)
                {
                    pastryRenderer.enabled = visible;
                }
            }
        }

        if (!visible && isMenuOpen)
        {
            selectionMenu.SetActive(false);
            isMenuOpen = false;
        }

        Debug.Log($"Pastry system visibility set to {visible}");
    }
}