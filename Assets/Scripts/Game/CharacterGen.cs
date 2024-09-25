using UnityEngine;
using System.Collections;   

public class CharacterGen : MonoBehaviour
{
    public float min = 0f;  // to test what the average waiting time would be
    public float max = 11f; 
    int dejavu = 0;
    int randomNumber = 1;
    void Update()
    {
        while(randomNumber != dejavu)
        {
           randomNumber = Random.Range((int)min, (int)max + 1); // yes this looks like it makes sense
           dejavu = randomNumber;
        }
    }
    
    
}
