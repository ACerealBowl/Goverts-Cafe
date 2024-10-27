using System.Collections;
using UnityEngine;

public class NiggaBalls : MonoBehaviour
{
    // Define the correct sequence of keys (you can change these to other keys)
    public KeyCode[] correctSequence;

    // Index to keep track of current position in the sequence
    private int sequenceIndex = 0;

    // Boolean to track whether the sequence is completed
    private bool sequenceComplete = false;

    void Start()
    {
        // change this
        if (correctSequence == null || correctSequence.Length == 0)
        {
            correctSequence = new KeyCode[] { KeyCode.UpArrow, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.B, KeyCode.A, KeyCode.Return, };
        }
    }

    void Update()
    {
        if (!sequenceComplete)
        {
            CheckInputSequence();
        }
    }

    // Function to check if the user is pressing the correct sequence of keys
    void CheckInputSequence()
    {
        if (Input.GetKeyDown(correctSequence[sequenceIndex]))
        {
            // If the correct key is pressed, move to the next key in the sequence
            sequenceIndex++;

            // Check if the sequence is completed
            if (sequenceIndex >= correctSequence.Length)
            {
                sequenceComplete = true;
                OnSequenceComplete();
            }
        }
        else if (Input.anyKeyDown)
        {
            // If a wrong key is pressed, reset the sequence
            sequenceIndex = 0;
        }
    }

    // This function will be called once the sequence is completed
    void OnSequenceComplete()
    {
        Debug.Log("Correct sequence entered!");
    }
}
