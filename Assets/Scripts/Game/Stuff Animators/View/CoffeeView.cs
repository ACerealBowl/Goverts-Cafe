using System.Collections;
using UnityEngine;

public class CoffeeView : MonoBehaviour
{
    [SerializeField] private GameObject Main;
    [SerializeField] private GameObject Coffee;
    [SerializeField] private CameraAnimator cameraAnimator;
    private void ShowContent()
    {
        StartCoroutine(CoffeeViewCoroutine());
    }

    private IEnumerator CoffeeViewCoroutine()
    {
        cameraAnimator.PlayCoffeeAnimation();

        yield return new WaitForSeconds(0.5f);
        Main.SetActive(false);
        Coffee.SetActive(true);
    }
}