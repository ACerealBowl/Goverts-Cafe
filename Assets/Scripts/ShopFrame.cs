using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopFrame : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(sigma());
    }

    private IEnumerator sigma()
    {
        yield return new WaitForSeconds(8.5f);
        if (DataManager.Instance.Endler) // Check stored value
        {
            SceneManager.LoadSceneAsync("Endless");
        }
    }
}
