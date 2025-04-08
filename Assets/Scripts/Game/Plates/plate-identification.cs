using UnityEngine;
using System;

public class PlateIdentifier : MonoBehaviour
{
    // Unique identifier for this plate
    [SerializeField] private string plateId;

    // Generate a unique ID if not set in inspector
    private void Awake()
    {
        if (string.IsNullOrEmpty(plateId))
        {
            plateId = Guid.NewGuid().ToString();
        }
    }

    // Getter for the plate's unique ID
    public string GetPlateId()
    {
        return plateId;
    }

    // Optional: Allow setting a custom ID if needed
    public void SetPlateId(string newId)
    {
        plateId = newId;
    }
}