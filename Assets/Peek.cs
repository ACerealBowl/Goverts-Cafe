using UnityEngine;

public class Peek : MonoBehaviour
{
    public Animator personAnimator;
    public string animationName = "Peek";
    public float min = 0f;
    public float max = 11f;
    public float interval = 1f;
    private float timer = 0f;

    private void Start()
    {
        if (personAnimator == null)
        {
            personAnimator = GetComponent<Animator>();
        }
        if (personAnimator == null)
        {
            Debug.LogError("Animator component not found!");
            return;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0f; // Reset the timer

            if (personAnimator.HasState(0, Animator.StringToHash(animationName)))
            {
                int randomNumber = Random.Range((int)min, (int)max + 1); // +1 to include the max value
                Debug.Log($"Random number: {randomNumber}");

                if (randomNumber == 3)
                {
                    personAnimator.Play(animationName, 0);
                    Debug.Log("Playing Peek animation");
                }
            }
            else
            {
                Debug.LogWarning($"Animation state '{animationName}' not found!");
            }
        }
    }
}