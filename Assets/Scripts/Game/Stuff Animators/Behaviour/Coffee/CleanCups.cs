using UnityEngine;

public class CleanCups : MonoBehaviour
{
    [SerializeField] private GameObject mtCup;
    [SerializeField] private GameObject cappuccino;
    [SerializeField] private CupSystem cupSystem;

    private GameObject activeEmptyCup;
    private GameObject activeFilledCup;

    private void Start()
    {
        if (mtCup != null)
        {
            mtCup.SetActive(false);
        }
    }

    private void Update()
    {
        UpdateCupPositions();
        HandleInput();
    }

    private void UpdateCupPositions()
    {
        if (activeEmptyCup != null || activeFilledCup != null)
        {
            Vector3 mouseWorldPos = GetMouseWorldPosition();

            if (activeEmptyCup != null && activeEmptyCup.activeSelf)
            {
                activeEmptyCup.transform.position = mouseWorldPos;
            }

            if (activeFilledCup != null)
            {
                activeFilledCup.transform.position = mouseWorldPos;
            }
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.z = 0;
        return worldPos;
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            CancelActiveCups();
        }
    }

    private void CancelActiveCups()
    {
        if (activeEmptyCup != null)
        {
            activeEmptyCup.SetActive(false);
            activeEmptyCup = null;
        }
        if (activeFilledCup != null)
        {
            Destroy(activeFilledCup);
            activeFilledCup = null;
        }
    }

    private void OnMouseDown()
    {
        if (gameObject.activeSelf && activeEmptyCup == null && activeFilledCup == null && !cupSystem.DeadCups)
        {
            activeEmptyCup = mtCup;
            activeEmptyCup.SetActive(true);
            activeEmptyCup.transform.position = GetMouseWorldPosition();
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

    public void SpawnFilledCup(Vector3 position, int cupPosition)
    {
        if (cappuccino != null && activeFilledCup == null)
        {
            Vector3 mousePos = GetMouseWorldPosition();
            activeFilledCup = Instantiate(cappuccino, mousePos, Quaternion.identity);

            FilledCup filledCupScript = activeFilledCup.GetComponent<FilledCup>();
            if (filledCupScript != null)
            {
                filledCupScript.SetCupPosition(cupPosition);
            }

            Debug.Log($"Spawned filled cup for position {cupPosition} at {mousePos}");
        }
    }
}