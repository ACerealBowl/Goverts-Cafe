using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateAnimationController : MonoBehaviour
{
    [SerializeField] private Animator plateAnimator;
    [SerializeField] private Pastry pastrySystem;

    // Current visibility state
    private bool platesVisible = true;

    private void Start()
    {
        if (pastrySystem == null)
        {
            pastrySystem = FindObjectOfType<Pastry>();
            if (pastrySystem == null)
            {
                Debug.LogError("Pastry system not found!");
            }
        }
    }

    public void SetPlatesAndPastriesVisibility(bool visible)
    {
        platesVisible = visible;
        GameManager.Instance.SetAllPlatesVisibility(visible);

        if (pastrySystem != null)
        {
            pastrySystem.SetVisibility(visible);
        }

        Debug.Log($"Set all plates and pastries visibility to {visible}");
    }

    public void TogglePlatesAndPastries()
    {
        SetPlatesAndPastriesVisibility(!platesVisible);
    }

    public void OnPlateAnimationStart()
    {
        // Animation start logic if needed
    }

    public void OnPlateAnimationEnd()
    {
        // Animation end logic if needed
    }
}