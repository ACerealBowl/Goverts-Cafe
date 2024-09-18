using UnityEngine;

public class NrGenerator : MonoBehaviour
{
    public float min = 0f;
    public float max = 11f;
    public float interval = 1f; // Log every 1 second

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            int randomNumber = Random.Range((int)min, (int)max);
            Debug.Log($"Attempt {randomNumber}"); // output to log instead of doin sumtin
            timer = 0f; // Reset the timer
        }
    }
}