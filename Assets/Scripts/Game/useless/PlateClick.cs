using UnityEngine;

public class PlateClick : MonoBehaviour
{
    private PlatesManager platesManager;

    private void Start()
    {
        platesManager = GetComponentInParent<PlatesManager>();
    }

    private void OnMouseDown()
    {
        if (platesManager != null)
        {
            platesManager.OnPlateClick(transform);
        }
    }
}