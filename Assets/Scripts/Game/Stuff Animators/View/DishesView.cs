using System.Collections;
using UnityEngine;

public class DishesView : MonoBehaviour
{
    [SerializeField] private GameObject Main;
    [SerializeField] private GameObject Dishes;
    [SerializeField] private CameraAnimator cameraAnimator;
    private void ShowContent()
    {
        StartCoroutine(DishesViewCoroutine());
    }

    private IEnumerator DishesViewCoroutine()
    {
        cameraAnimator.PlayDishesAnimation();

        yield return new WaitForSeconds(0.5f);
        Main.SetActive(false);
        Dishes.SetActive(true);
    }
}