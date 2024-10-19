using System.Collections;
using UnityEngine;

public class PastryView : MonoBehaviour
{
    [SerializeField] private GameObject Main;
    [SerializeField] private GameObject Pastry;
    [SerializeField] private CameraAnimator cameraAnimator;
    private void ShowContent()
    {
        StartCoroutine(PastryViewCoroutine());
    }

    private IEnumerator PastryViewCoroutine()
    {
        cameraAnimator.PlayPastryAnimation();

        yield return new WaitForSeconds(0.5f);
        Main.SetActive(false);
        Pastry.SetActive(true);
    }
}