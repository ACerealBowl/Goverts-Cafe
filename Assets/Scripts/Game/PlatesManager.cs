using UnityEngine;

public class PlatesManager : MonoBehaviour
{
    [SerializeField] private GameObject mtCup;      // Original mtCup prefab with scripts
    [SerializeField] private GameObject Cappucino;  // Original Cappucino prefab with scripts
    [SerializeField] private Transform Leftplate;
    [SerializeField] private Transform Rightplate;
    [SerializeField] private Transform Topplate;


    // Called when clicking on plates
    public void OnPlateClick(Transform plate)
    {
        Vector3 spawnPosition;
        GameObject prefabToSpawn;

        if (plate == Leftplate)
        {
            spawnPosition = new Vector3(6.28f, -2.64f, 0);
            prefabToSpawn = mtCup;
        }
        else if (plate == Rightplate)
        {
            spawnPosition = new Vector3(1.58f, -2.58f, 0);
            prefabToSpawn = mtCup;
        }
        else if (plate == Topplate)
        {
            spawnPosition = new Vector3(3.36f, -1.65f, 0);
            prefabToSpawn = Cappucino;
        }
        else
        {
            return;
        }

        // Create the cup
        GameObject newCup = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        newCup.transform.SetParent(plate);
    }
}