using UnityEngine;
using System.Collections;   

public class CharacterGen : MonoBehaviour
{
    public float min = 1;  // to test what the average waiting time would be
    public float max = 20; 
    int dejavu = 0;
    int dejavu2 = 0;
    int dejavu3 = 0;
    int randomNumber = 1;
    void Update()
    {
        while(randomNumber != dejavu && != dejavu2 && != dejavu3)
        {
           randomNumber = Random.Range((int)min, (int)max + 1); // yes this looks like it makes sense
           dejavu = randomNumber;
        }

    }
    
    
}
