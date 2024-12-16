using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public bool Endler; // Shared data

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
