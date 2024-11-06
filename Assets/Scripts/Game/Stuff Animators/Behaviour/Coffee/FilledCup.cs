using UnityEngine;

public class FilledCup : MonoBehaviour
{
    private void Update()
    {
        // Only update position if the object is active
        if (gameObject.activeSelf)
        {
            FollowMouse();
        }
    }

    private void FollowMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -Camera.main.transform.position.z; // Set depth to match main camera's view
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 0; // Keep the object on the same plane

        transform.position = worldPosition;
    }

    public void SetCupPosition(int position)
    {
        // Optional: Additional logic if you need to set specific positions
        Debug.Log($"Cup position set to {position}");
    }
}
