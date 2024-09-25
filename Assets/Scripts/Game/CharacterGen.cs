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
    public float waitimg = 20f;
    private void Start()
    {
        StartCoroutine(GenerateRandomNumber());
    }

    private IEnumerator GenerateRandomNumber()
    {
        while (true)
        {
            do
            {
                randomNumber = Random.Range((int)min, (int)max + 1); // yes this looks like it makes sense
            } while (randomNumber == dejavu || randomNumber == dejavu2 || randomNumber == dejavu3);

                dejavu3 = dejavu2;
                dejavu2 = dejavu;
                dejavu = randomNumber;

                yield return new WaitForSeconds(waitimg);
                //testing testing 1212 Debug.Log("Sigma Sigma On The Wall:" + randomNumber);
        }

    }
    
    
}
