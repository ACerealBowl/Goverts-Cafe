using UnityEngine;
using System.Collections;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] private Camera logoCamera;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject uiCanvas;
    [SerializeField] private GameObject sigma;
    [SerializeField] private float switchDelay = 8f;
    [Header("---Skip that Garbage---")]
    public bool SkipIntro = false;

    private void Start()
    {
        if (logoCamera == null || mainCamera == null)
        {
            Debug.LogError("Where the camera blud?");
            return;
        }

        if (uiCanvas == null)
        {
            Debug.LogWarning("buttons no seen bro");
        }

        if(SkipIntro != true) // just incase I wanna skip the intro to test sum
        {
            // Initial setup
            logoCamera.enabled = true;
            mainCamera.enabled = false;

            // Turn off UI canvas and turn on Sigma
            if (uiCanvas != null)
            {
                uiCanvas.SetActive(false);
            }
            sigma.SetActive(true);

            // Start the camera switch coroutine
            StartCoroutine(SwitchToMainCamera());
        }
        else // does everything with no delay
        {
            logoCamera.enabled = false;
            mainCamera.enabled = true;

            if (uiCanvas != null)
            {
                uiCanvas.SetActive(true);
            }
            sigma.SetActive(false);
        }
        
    }

    private IEnumerator SwitchToMainCamera()
    {
        yield return new WaitForSeconds(switchDelay);

        // Switch cameras
        logoCamera.enabled = false;
        mainCamera.enabled = true;

        // Enable the UI Canvas and disable Sigma
        if (uiCanvas != null)
        {
            uiCanvas.SetActive(true);
        }
        sigma.SetActive(false);
    }
}