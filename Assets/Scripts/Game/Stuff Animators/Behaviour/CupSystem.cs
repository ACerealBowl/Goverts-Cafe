using UnityEngine;
using System.Collections;

public class CupSystem : MonoBehaviour
{
    [SerializeField] private GameObject singleCup;
    [SerializeField] private Animator cupsAnimator;
    public int requiredCups = 0;
    private float updateInterval = 30f;
    [SerializeField] private CameraAnimator cameraAnimator;

    private void Start()
    {
        if (singleCup != null)
            singleCup.SetActive(false);

        // Initial cup requirement
        AddRandomCups();
        StartCoroutine(UpdateRequiredCupsRoutine());
    }

    private void Update()
    {
        if (singleCup.activeSelf)
        {
            Vector3 mousePos = Input.mousePosition; // singular cup movement idunno
            mousePos.z = -Camera.main.transform.position.z;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            singleCup.transform.position = worldPos;
        }
    }

    private void OnMouseDown()
    {
        if (gameObject.activeSelf && !singleCup.activeSelf)
        {
            singleCup.SetActive(true);
        }
    }

    private void AddRandomCups()
    {
        if (requiredCups < 20)
        {
            int additionalCups = Random.Range(3, 7);
            requiredCups += additionalCups;
            Debug.Log($"Added {additionalCups} Cups!");
        }
        else
        {
            Debug.LogWarning($"Cup requirement {requiredCups} has exceeded 20!");
        }

        // Trigger animation to show new cups added and check for annoying ass tab
        if (cupsAnimator != null)
        {
            if (GetDishesMenuState())
            cupsAnimator.SetTrigger("Show");
        }
    }
    private bool GetDishesMenuState() // check if menu is on (ty domnul de info)
    {
        return cameraAnimator.DishesMenu;
    }

    private IEnumerator UpdateRequiredCupsRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(updateInterval);
            AddRandomCups();
        }
    }

    public void ProcessCup()
    {
        requiredCups--;
        singleCup.SetActive(false);

        if (requiredCups == 0)
        {
            if (cupsAnimator != null)
            {
                cupsAnimator.SetTrigger("Hide");
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}