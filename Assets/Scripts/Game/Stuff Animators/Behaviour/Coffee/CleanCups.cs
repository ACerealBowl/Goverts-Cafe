using UnityEngine;
using System.Collections;

public class CleanCups : MonoBehaviour
{
        
    [SerializeField] private GameObject singularCup;
    [SerializeField] private GameObject filledCup;
    [SerializeField] private Animator cupsAnimator;
    [SerializeField] private CupSystem cupSystem;
    private GameObject activeEmptyCup;
    private GameObject activeFilledCup;
    private bool isAnimationCoroutineRunning = false;
    private void Start()
    {
        if (singularCup != null)
            singularCup.SetActive(false);
        if (filledCup != null)
            filledCup.SetActive(false);
    }

    private void Update()
    {
        // Handle empty cup movement
        if (activeEmptyCup != null && activeEmptyCup.activeSelf)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -Camera.main.transform.position.z;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            activeEmptyCup.transform.position = worldPos;
        }

        // Handle filled cup movement
        if (activeFilledCup != null && activeFilledCup.activeSelf)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -Camera.main.transform.position.z;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            activeFilledCup.transform.position = worldPos;
        }

        if (Input.GetMouseButtonDown(1))  // 1 is right mouse button!!!1!1
        {
            ProcessFilledCup();  // This will remove the cup ig
        }

        if (!isAnimationCoroutineRunning)
        {
            isAnimationCoroutineRunning = true;
            if (cupSystem.DeadCups)
            {
                StartCoroutine(ExceededCups());
            }
            else
            {
                StartCoroutine(NormalCups());
            }
        }
    }

    private IEnumerator ExceededCups()
    {
        yield return new WaitForSeconds(60f);
       cupsAnimator.SetTrigger("ExceedLimit");
        isAnimationCoroutineRunning = false;
    }
    private IEnumerator NormalCups()
    {
        yield return new WaitForSeconds(60f);
        cupsAnimator.SetTrigger("idle");
        isAnimationCoroutineRunning = false;
    }

    private void OnMouseDown()
    {
        if (gameObject.activeSelf && activeEmptyCup == null)
        {
            activeEmptyCup = singularCup;
            activeEmptyCup.SetActive(true);
        }
    }

    public void ProcessEmptyCup()
    {
        if (activeEmptyCup != null)
        {
            activeEmptyCup.SetActive(false);
            activeEmptyCup = null;
        }
    }

    public void SpawnFilledCup(Vector3 position)
    {
        if (filledCup != null && activeFilledCup == null)
        {
            activeFilledCup = filledCup;
            activeFilledCup.transform.position = position;
            activeFilledCup.SetActive(true);
        }
    }
    public void ProcessFilledCup()
    {
        if (activeFilledCup != null)
        {
            activeFilledCup.SetActive(false);
            activeFilledCup = null;
        }
    }
}