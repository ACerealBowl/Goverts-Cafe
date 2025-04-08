using System;
using UnityEngine;

public class CoffeeMachine : MonoBehaviour
{
    [SerializeField] private Animator coffeeAnimator;
    [SerializeField] private CleanCups cleanCupsSystem;
    [SerializeField] private GameObject cappuccino;
    [SerializeField] private Vector3 leftSlotPosition;
    [SerializeField] private Vector3 rightSlotPosition;
    private const float BREW_TIME = 8f;

    [Header("Debug Settings")]
    [SerializeField] private bool debugMode = true;

    private class CupSlot
    {
        public bool isPresent;
        public bool isFilled;
        public float brewTimer;
        public Vector3 position;

        public void Reset()
        {
            isPresent = false;
            isFilled = false;
            brewTimer = 0f;
        }

        public void StartBrewing(Vector3 pos)
        {
            isPresent = true;
            isFilled = false;
            brewTimer = 0f;
            position = pos;
        }
    }

    private CupSlot[] slots = new CupSlot[2] { new CupSlot(), new CupSlot() };

    private void Start()
    {
        // Reset animation state at start
        if (coffeeAnimator != null)
        {
            coffeeAnimator.SetTrigger("idle");
        }

        if (debugMode)
        {
            Debug.Log("[CoffeeMachine] Started with left slot position: " + leftSlotPosition +
                      " and right slot position: " + rightSlotPosition);
        }
    }

    private void Update()
    {
        bool stateChanged = false;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].isPresent && !slots[i].isFilled)
            {
                slots[i].brewTimer += Time.deltaTime;
                if (slots[i].brewTimer >= BREW_TIME)
                {
                    slots[i].isFilled = true;
                    stateChanged = true;
                    Debug.Log($"Cup in slot {i} is now filled");
                }
            }
        }

        if (stateChanged)
        {
            UpdateAnimation();
        }
    }

    private void OnMouseDown()
    {
        Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (debugMode)
        {
            Debug.Log($"[CoffeeMachine] Click Position: {clickPosition}");
            Debug.Log($"[CoffeeMachine] Coffee Machine Position: {transform.position}");
        }

        int position;
        if (TryRemoveCup(clickPosition, out position))
        {
            Debug.Log($"[CoffeeMachine] Successfully removed cup from slot {position}");
        }
        else if (debugMode)
        {
            // Debug why removal failed
            Debug.Log($"[CoffeeMachine] Cup removal failed. Slot {position} status: " +
                      $"isPresent={slots[position].isPresent}, isFilled={slots[position].isFilled}");
        }
    }

    public bool CanAcceptCup()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int slotIndex = mousePos.x < transform.position.x ? 0 : 1;

        if (debugMode)
        {
            Debug.Log($"[CoffeeMachine] CanAcceptCup check: Mouse position {mousePos}, " +
                      $"machine position {transform.position}, slot {slotIndex}, " +
                      $"result: {!slots[slotIndex].isPresent}");
        }

        return !slots[slotIndex].isPresent;
    }

    public void AddCup(Vector3 position)
    {
        int slotIndex = (Vector3.Distance(position, leftSlotPosition) < Vector3.Distance(position, rightSlotPosition)) ? 0 : 1;

        if (debugMode)
        {
            Debug.Log($"[CoffeeMachine] AddCup attempt at position {position}, " +
                      $"distance to left: {Vector3.Distance(position, leftSlotPosition)}, " +
                      $"distance to right: {Vector3.Distance(position, rightSlotPosition)}, " +
                      $"selected slot: {slotIndex}, slot available: {!slots[slotIndex].isPresent}");
        }

        if (!slots[slotIndex].isPresent)
        {
            slots[slotIndex].StartBrewing(slotIndex == 0 ? leftSlotPosition : rightSlotPosition);
            UpdateAnimation();
            Debug.Log($"[CoffeeMachine] Added cup to slot {slotIndex} at position {(slotIndex == 0 ? leftSlotPosition : rightSlotPosition)}");
        }
        else if (debugMode)
        {
            Debug.LogWarning($"[CoffeeMachine] Failed to add cup: slot {slotIndex} is already occupied");
        }
    }

    public bool TryRemoveCup(Vector3 clickPosition, out int position)
    {
        position = clickPosition.x < transform.position.x ? 0 : 1;

        if (debugMode)
        {
            Debug.Log($"[CoffeeMachine] TryRemoveCup: Position {position}, " +
                      $"isPresent={slots[position].isPresent}, isFilled={slots[position].isFilled}");
        }

        if (slots[position].isPresent && slots[position].isFilled)
        {
            Vector3 spawnPos = slots[position].position;
            cleanCupsSystem.SpawnFilledCup(spawnPos, position);
            slots[position].Reset();
            UpdateAnimation();

            if (cappuccino != null)
            {
                cappuccino.SetActive(true);
            }
            else
            {
                Debug.LogWarning("[CoffeeMachine] Cappuccino reference is missing in CoffeeMachine.");
            }
            return true;
        }
        return false;
    }

    private void UpdateAnimation()
    {
        if (coffeeAnimator == null) return;

        // Reset all triggers first
        coffeeAnimator.ResetTrigger("idle");
        coffeeAnimator.ResetTrigger("LeftCup");
        coffeeAnimator.ResetTrigger("RightCup");
        coffeeAnimator.ResetTrigger("LeftCupFilled");
        coffeeAnimator.ResetTrigger("RightCupFilled");
        coffeeAnimator.ResetTrigger("BothCupsFilled");
        coffeeAnimator.ResetTrigger("LeftFilledRightNot");
        coffeeAnimator.ResetTrigger("RightFilledLeftNot");
        coffeeAnimator.ResetTrigger("BothCupsEmpty");

        string triggerName = DetermineAnimationState();
        if (debugMode)
        {
            Debug.Log($"[CoffeeMachine] Setting animation trigger: {triggerName}");
        }
        coffeeAnimator.SetTrigger(triggerName);
    }

    private string DetermineAnimationState()
    {
        if (!slots[0].isPresent && !slots[1].isPresent)
            return "idle";

        if (slots[0].isPresent && !slots[1].isPresent)
            return slots[0].isFilled ? "LeftCupFilled" : "LeftCup";

        if (!slots[0].isPresent && slots[1].isPresent)
            return slots[1].isFilled ? "RightCupFilled" : "RightCup";

        if (slots[0].isPresent && slots[1].isPresent)
        {
            if (slots[0].isFilled && slots[1].isFilled)
                return "BothCupsFilled";
            if (slots[0].isFilled && !slots[1].isFilled)
                return "LeftFilledRightNot";
            if (!slots[0].isFilled && slots[1].isFilled)
                return "RightFilledLeftNot";
            return "BothCupsEmpty";
        }

        return "idle";
    }

    public void DebugSlotStatus()
    {
        Debug.Log("===== COFFEE MACHINE SLOT STATUS =====");
        Debug.Log($"Left Slot (0): Present={slots[0].isPresent}, Filled={slots[0].isFilled}, Timer={slots[0].brewTimer}");
        Debug.Log($"Right Slot (1): Present={slots[1].isPresent}, Filled={slots[1].isFilled}, Timer={slots[1].brewTimer}");
        Debug.Log("=====================================");
    }

    // Add debug method to check collisions
    private void OnDrawGizmos()
    {
        if (debugMode)
        {
            // Draw slot positions
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(leftSlotPosition, 0.1f);
            Gizmos.DrawSphere(rightSlotPosition, 0.1f);

            // Draw coffee machine collider
            Gizmos.color = Color.green;
            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            if (collider != null)
            {
                Gizmos.DrawWireCube(transform.position + (Vector3)collider.offset, collider.size);
            }
        }
    }
}