using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pastry : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject cake;  // This will be our single cake instance
    [SerializeField] private Sprite[] pastrySprites;
    [SerializeField] private GameObject selectionMenu;
    [SerializeField] private Button[] selectionButtons;
    [SerializeField] private Button exitButton;
    [SerializeField] private Animator menuAnimator;

    private bool isMenuOpen = false;
    private static bool isItemActive = false;  // Similar to CupSystem's pattern

    private void Start()
    {
        // Initialize cake as inactive, like in CupSystem
        if (cake != null)
            cake.SetActive(false);

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

        if (exitButton != null)
        {
            exitButton.onClick.AddListener(ExitAndDestroy);
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

    private void Update()
    {
        // Update cake position when active, similar to CupSystem's Update
        if (cake != null && cake.activeSelf)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -Camera.main.transform.position.z;
            cake.transform.position = Camera.main.ScreenToWorldPoint(mousePos);

            // Right-click to deactivate
            if (Input.GetMouseButtonDown(1))
            {
                DeactivateCake();
            }
        }
    }

    private void OnMouseDown()
    {
        // Only show menu if no cake is active and menu isn't open
        if (gameObject.activeSelf && !isItemActive && !isMenuOpen)
        {
            CenterMenu();
            selectionMenu.SetActive(true);
            menuAnimator.SetTrigger("open");
            isMenuOpen = true;
        }
    }

    private void SelectPastry(int index)
    {
        if (!isItemActive && cake != null)
        {
            if (spriteRenderer != null && pastrySprites != null && index < pastrySprites.Length)
            {
                spriteRenderer.sprite = pastrySprites[index];
            }

            // Activate and position the cake
            cake.SetActive(true);
            isItemActive = true;

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            cake.transform.position = mousePosition;
        }

        selectionMenu.SetActive(false);
        isMenuOpen = false;
    }

    private void DeactivateCake()
    {
        if (cake != null)
        {
            cake.SetActive(false);
            isItemActive = false;
        }
    }

    public void ExitAndDestroy()
    {
        DeactivateCake();

        if (selectionMenu != null)
        {
            selectionMenu.SetActive(false);
            isMenuOpen = false;
        }
    }

    private void OnDisable()
    {
        DeactivateCake();
    }
}