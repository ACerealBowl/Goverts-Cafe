using UnityEngine;
using System.Collections;
using TMPro;
using TMPro.Examples;

public class CupSystem : MonoBehaviour
{
    [SerializeField] private GameObject cup;
    [SerializeField] private GameObject dishwasher;
    [SerializeField] private GameObject cupsHD;
    [SerializeField] private Animator cupsAnimator;
    [SerializeField] private CameraAnimator cameraAnimator;
    [SerializeField] private Animator nrAnimator;
    public int dirtyCupsCount = 0;
    private const int MAX_CUPS = 20;
    private GameObject activeDirtyCup;
    public TMP_Text numberText;
    public bool DeadCups => dirtyCupsCount >= MAX_CUPS;
    public bool HasNoCups => dirtyCupsCount <= 0;

    public void Start()
    {
        if (cup != null)
            cup.SetActive(false);
        AddRandomCups();
        StartCoroutine(UpdateRequiredCupsRoutine());
        UpdateCleanCupsVisibility();
    }

    private void Update()
    {
        if (activeDirtyCup != null && activeDirtyCup.activeSelf)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -Camera.main.transform.position.z;
            activeDirtyCup.transform.position = Camera.main.ScreenToWorldPoint(mousePos);
        }
    }

    private void OnMouseDown()
    {
        if (gameObject.activeSelf && activeDirtyCup == null)
        {
            activeDirtyCup = cup;
            activeDirtyCup.SetActive(true);
        }
    }

    private void AddRandomCups()
    {
        if (dirtyCupsCount < MAX_CUPS)
        {
            int additionalCups = Random.Range(3, 7);
            dirtyCupsCount = Mathf.Min(dirtyCupsCount + additionalCups, MAX_CUPS);
            Debug.Log($"Added {additionalCups} dirty cups. Total: {dirtyCupsCount}");
            numberText.text = dirtyCupsCount.ToString(); // Update the text i hope
            if (GetDishesMenuState( ))
                cupsAnimator.SetTrigger("Show");
            UpdateCleanCupsVisibility();
        }
    }
    private bool GetDishesMenuState() // check if menu is on (ty domnul de info)
    {
        return cameraAnimator.DishesMenu;
    }

    public void fixRetard()
    {
        if (GetDishesMenuState())
            Debug.Log($"NIGGER");
        else
        {
            cupsAnimator.SetTrigger("Hide");
            StartCoroutine(UpdateRequiredCupsRoutine());
        }

        }

    public void UpdateCleanCupsVisibility()
    {
        if (cupsHD != null)
            cupsHD.SetActive(!DeadCups);

        if (cupsAnimator != null)
        {
            if (HasNoCups)
            {
                cupsAnimator.SetTrigger("Hide");
            }
        }
    }

    private IEnumerator UpdateRequiredCupsRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(30f);
            AddRandomCups();
            UpdateCleanCupsVisibility();
        }
    }

    public void ProcessDirtyCup()
    {
        if (activeDirtyCup != null)
        {
            dirtyCupsCount--;
            numberText.text = dirtyCupsCount.ToString();
            nrAnimator.SetTrigger("click");
            activeDirtyCup.SetActive(false);
            activeDirtyCup = null;
            UpdateCleanCupsVisibility();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == dishwasher)
        {
            ProcessDirtyCup();
        }
    }
}