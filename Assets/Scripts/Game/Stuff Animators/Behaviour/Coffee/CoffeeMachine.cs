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
    private class CupSlot //I turned off a bunch of log updates cause I got it to work some time ago #cleanyourconsole or something
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
        //Debug.Log($"Click Position: {clickPosition}");
       // Debug.Log($"Coffee Machine Position: {transform.position}");
        int position;
        if (TryRemoveCup(clickPosition, out position))
        {
            Debug.Log($"Successfully removed cup from slot {position}");
        }
    }

    public bool CanAcceptCup()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int slotIndex = mousePos.x < transform.position.x ? 0 : 1;
        return !slots[slotIndex].isPresent;
    }

    public void AddCup(Vector3 position)
    {
        int slotIndex = (Vector3.Distance(position, leftSlotPosition) < Vector3.Distance(position, rightSlotPosition)) ? 0 : 1;

        if (!slots[slotIndex].isPresent)
        {
            slots[slotIndex].StartBrewing(slotIndex == 0 ? leftSlotPosition : rightSlotPosition);
            UpdateAnimation();
            Debug.Log($"Added cup to slot {slotIndex} at position {(slotIndex == 0 ? leftSlotPosition : rightSlotPosition)}");
        }
    }

    public bool TryRemoveCup(Vector3 clickPosition, out int position)
    {
        position = clickPosition.x < transform.position.x ? 0 : 1;

        if (slots[position].isPresent && slots[position].isFilled)
        {
            Vector3 spawnPos = slots[position].position;
            cleanCupsSystem.SpawnFilledCup(spawnPos, position);
            slots[position].Reset();
            UpdateAnimation();

            if (cappuccino != null)
            {
                cappuccino.SetActive(true);
                //Debug.Log($"Cappuccino activated after cup removal from slot {position}!");
            }
            else
            {
                Debug.LogWarning("Cappuccino reference is missing in CoffeeMachine.");
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
        //Debug.Log($"Setting animation trigger: {triggerName}");
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
}