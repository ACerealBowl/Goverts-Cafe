using UnityEngine;

public class CleanCups : MonoBehaviour
{
    [SerializeField] private GameObject mtCup;
    [SerializeField] private GameObject cappuccino;
    [SerializeField] private CupSystem cupSystem;

    [Header("Debug Settings")]
    [SerializeField] private bool debugMode = true;

    private GameObject activeEmptyCup;
    private GameObject activeFilledCup;
    private Pastry pastrySystem; // Reference to check pastry system state

    private void Awake()
    {
        pastrySystem = FindObjectOfType<Pastry>();

        if (debugMode)
        {
            Debug.Log($"[CleanCups] Awake - Found pastry system: {pastrySystem != null}");
        }
    }

    private void Start()
    {
        if (mtCup != null)
        {
            mtCup.SetActive(false);
        }
        else if (debugMode)
        {
            Debug.LogError("[CleanCups] mtCup reference is missing!");
        }

        if (debugMode)
        {
            Debug.Log($"[CleanCups] Started - mtCup: {mtCup != null}, cappuccino: {cappuccino != null}, cupSystem: {cupSystem != null}");

            if (cupSystem != null)
            {
                Debug.Log($"[CleanCups] Cup system dead cups state: {cupSystem.DeadCups}");
            }
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
            if (debugMode && (activeEmptyCup != null || activeFilledCup != null))
            {
                Debug.Log("[CleanCups] Right click detected - canceling active cups");
            }
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
        if (debugMode)
        {
            Debug.Log($"[CleanCups] OnMouseDown - gameObject.activeSelf: {gameObject.activeSelf}, " +
                      $"activeEmptyCup: {activeEmptyCup != null}, activeFilledCup: {activeFilledCup != null}, " +
                      $"cupSystem.DeadCups: {(cupSystem != null ? cupSystem.DeadCups.ToString() : "cupSystem null")}");

            // Check if pastry system is active
            if (pastrySystem != null)
            {
                Debug.Log("[CleanCups] Pastry system exists. Check if it's active or handling input.");
            }
        }

        bool canSpawnCup = gameObject.activeSelf &&
                           activeEmptyCup == null &&
                           activeFilledCup == null &&
                           (cupSystem == null || !cupSystem.DeadCups);

        if (canSpawnCup)
        {
            if (debugMode)
            {
                Debug.Log("[CleanCups] Spawning empty cup");
            }

            activeEmptyCup = mtCup;
            activeEmptyCup.SetActive(true);
            activeEmptyCup.transform.position = GetMouseWorldPosition();
        }
        else if (debugMode)
        {
            Debug.Log($"[CleanCups] Cannot spawn cup. Reason: " +
                     $"{(gameObject.activeSelf ? "" : "gameObject inactive, ")}" +
                     $"{(activeEmptyCup == null ? "" : "activeEmptyCup exists, ")}" +
                     $"{(activeFilledCup == null ? "" : "activeFilledCup exists, ")}" +
                     $"{(cupSystem == null ? "cupSystem null" : (cupSystem.DeadCups ? "cupSystem.DeadCups=true" : ""))}");
        }
    }

    public void ProcessEmptyCup()
    {
        if (debugMode)
        {
            Debug.Log($"[CleanCups] ProcessEmptyCup called. activeEmptyCup: {activeEmptyCup != null}");
        }

        if (activeEmptyCup != null)
        {
            activeEmptyCup.SetActive(false);
            activeEmptyCup = null;
        }
    }

    public void SpawnFilledCup(Vector3 position, int cupPosition)
    {
        if (debugMode)
        {
            Debug.Log($"[CleanCups] SpawnFilledCup called at position {position}, cupPosition {cupPosition}. " +
                      $"cappuccino: {cappuccino != null}, activeFilledCup: {activeFilledCup != null}");
        }

        if (cappuccino != null && activeFilledCup == null)
        {
            Vector3 mousePos = GetMouseWorldPosition();
            activeFilledCup = Instantiate(cappuccino, mousePos, Quaternion.identity);

            FilledCup filledCupScript = activeFilledCup.GetComponent<FilledCup>();
            if (filledCupScript != null)
            {
                filledCupScript.SetCupPosition(cupPosition);
            }
            else if (debugMode)
            {
                Debug.LogError("[CleanCups] FilledCup component missing on spawned cappuccino!");
            }

            Debug.Log($"[CleanCups] Spawned filled cup for position {cupPosition} at {mousePos}");
        }
        else if (debugMode && cappuccino == null)
        {
            Debug.LogError("[CleanCups] Cappuccino reference is missing!");
        }
    }

    // Method to debug the state
    public void DebugState()
    {
        Debug.Log("===== CLEAN CUPS SYSTEM STATE =====");
        Debug.Log($"Active Empty Cup: {activeEmptyCup != null}");
        Debug.Log($"Active Filled Cup: {activeFilledCup != null}");
        Debug.Log($"Cup System Dead Cups: {(cupSystem != null ? cupSystem.DeadCups.ToString() : "cupSystem null")}");
        Debug.Log($"Game Object Active: {gameObject.activeSelf}");
        Debug.Log("==================================");
    }
}