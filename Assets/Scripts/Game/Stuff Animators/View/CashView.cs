using System.Collections;
using UnityEngine;

public class CashView : MonoBehaviour
{
    [SerializeField] private GameObject Main;
    [SerializeField] private GameObject Cash;
    [SerializeField] private CameraAnimator cameraAnimator;
    private void ShowContent()
    {
        StartCoroutine(CashViewCoroutine());
    }

    private IEnumerator CashViewCoroutine()
    {
        cameraAnimator.PlayCashRegisterAnimation();

        yield return new WaitForSeconds(0.5f);
        Main.SetActive(false);
        Cash.SetActive(true);
    }
}