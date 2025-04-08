using UnityEngine;
using UnityEngine.UI;

// Add this script to a GameObject in your scene to get runtime debugging controls
public class CafeDebugger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CoffeeMachine coffeeMachine;
    [SerializeField] private CleanCups cleanCupsSystem;
    [SerializeField] private Pastry pastrySystem;
    [SerializeField] private PlateSystem plateSystem;

    [Header("Debug UI")]
    [SerializeField] private bool showDebugUI = true;
    [SerializeField] private GameObject debugPanel;
    [SerializeField] private Text statusText;

    [Header("Debug Options")]
    [SerializeField] private KeyCode toggleDebugKey = KeyCode.F1;
    [SerializeField] private KeyCode dumpStateKey = KeyCode.F2;
    [SerializeField] private KeyCode checkInteractionKey = KeyCode.F3;

    private void Start()
    {
        // Find references if not assigned
        if (coffeeMachine == null) coffeeMachine = FindObjectOfType<CoffeeMachine>();
        if (cleanCupsSystem == null) cleanCupsSystem = FindObjectOfType<CleanCups>();
        if (pastrySystem == null) pastrySystem = FindObjectOfType<Pastry>();
        if (plateSystem == null) plateSystem = FindObjectOfType<PlateSystem>();

        Debug.Log("[CafeDebugger] Debugger initialized");

        // Set up debug UI
        if (debugPanel != null)
        {
            debugPanel.SetActive(showDebugUI);
        }

        DumpState();
    }

    private void Update()
    {
        // Toggle debug UI
        if (Input.GetKeyDown(toggleDebugKey))
        {
            showDebugUI = !showDebugUI;
            if (debugPanel != null)
            {
                debugPanel.SetActive(showDebugUI);
            }
            Debug.Log($"[CafeDebugger] Debug UI {(showDebugUI ? "shown" : "hidden")}");
        }

        // Dump state to console
        if (Input.GetKeyDown(dumpStateKey))
        {
            DumpState();
        }

        // Check interactions
        if (Input.GetKeyDown(checkInteractionKey))
        {
            CheckInteractions();
        }

        // Update status text
        if (showDebugUI && statusText != null)
        {
            UpdateStatusText();
        }
    }

    private void DumpState()
    {
        Debug.Log("=============== CAFE DEBUGGER - STATE DUMP ===============");

        // Coffee Machine State
        Debug.Log("[CoffeeMachine] Status:");
        if (coffeeMachine != null)
        {
            coffeeMachine.DebugSlotStatus();
        }
        else
        {
            Debug.Log("CoffeeMachine not found");
        }

        // Clean Cups System
        Debug.Log("[CleanCups] Status:");
        if (cleanCupsSystem != null)
        {
            cleanCupsSystem.DebugState();
        }
        else
        {
            Debug.Log("CleanCupsSystem not found");
        }

        // Pastry System
        Debug.Log("[Pastry] Status:");
        if (pastrySystem != null)
        {
            Debug.Log("Pastry system found - implement pastry debug state if needed");
            // If Pastry has a debug method, call it here
        }
        else
        {
            Debug.Log("Pastry system not found");
        }

        Debug.Log("=========================================================");
    }

    private void CheckInteractions()
    {
        Debug.Log("========== CAFE DEBUGGER - INTERACTION CHECKS ==========");

        // Check if coffee machine can accept cups right now
        if (coffeeMachine != null)
        {
            Debug.Log($"[Interaction] Coffee machine can accept cups: {coffeeMachine.CanAcceptCup()}");
        }

        // Check input handling priorities
        Debug.Log("[Interaction] Input handlers active:");

        // Check if pastry system is currently handling input
        if (pastrySystem != null)
        {
            Debug.Log("- Pastry system exists (implement a way to check if it's handling input)");
        }

        // Check raycasts at mouse position to see what would be hit
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        RaycastHit2D[] hits = Physics2D.RaycastAll(worldPos, Vector2.zero);

        Debug.Log($"[Interaction] Raycast at mouse position ({worldPos}) hits {hits.Length} colliders:");
        foreach (RaycastHit2D hit in hits)
        {
            Debug.Log($"- Hit: {hit.collider.gameObject.name}, Tag: {hit.collider.tag}");
        }

        Debug.Log("=========================================================");
    }

    private void UpdateStatusText()
    {
        if (statusText == null) return;

        string text = "Cafe Debugger Status:\n";

        // Coffee Machine Status
        text += "Coffee Machine: " + (coffeeMachine != null ? "Found" : "Not Found") + "\n";

        // Clean Cups Status
        text += "Clean Cups: " + (cleanCupsSystem != null ? "Found" : "Not Found") + "\n";

        // Pastry System Status
        text += "Pastry System: " + (pastrySystem != null ? "Found" : "Not Found") + "\n";

        // Current mouse coords
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
        text += $"Mouse: ({mouseWorldPos.x:F2}, {mouseWorldPos.y:F2})\n";

        statusText.text = text;
    }

    // Add a GUI button to create a cup for testing
    public void CreateTestCup()
    {
        if (cleanCupsSystem != null)
        {
            // Force-spawn an empty cup
            Debug.Log("[CafeDebugger] Forcing cup creation");
            // Call a method on CleanCups to create a cup - you'd need to add this method
            // cleanCupsSystem.ForceCreateCup();
        }
    }
}