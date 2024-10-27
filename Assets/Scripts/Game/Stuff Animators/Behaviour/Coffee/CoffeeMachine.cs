using UnityEngine;
using System.Collections;
public class CoffeeMachine : MonoBehaviour
{
    [SerializeField] private Animator coffeeAnimator;
    public float time = 8f;
    private int currentCups = 0;
    private const int MAX_CUPS = 2;
    private float cupTimer = 0f;
    private bool cupReady = false;

    private void Update()
    {
        if (currentCups > 0 && !cupReady)
        {
            cupTimer += Time.deltaTime;
            if (cupTimer >= time)
            {
                cupReady = true;
                Debug.Log($"Coffee RISE");
            }
        }
    }

    public bool CanAcceptCup()
    {
        return currentCups < MAX_CUPS;
    }

    public bool CanRemoveCup()
    {
        return cupReady && currentCups > 0;
    }

    public void AddCup()
    {
        if (CanAcceptCup())
        {
            currentCups++;
            cupTimer = 0f;
            cupReady = false;
            UpdateAnimation();
        }
    }

    public void RemoveCup()
    {
        if (currentCups > 0)
        {
            currentCups--;
            cupReady = false;
            cupTimer = 0f;
            UpdateAnimation();
        }
    }

    private void UpdateAnimation()
    {
        if (coffeeAnimator != null)
        {
            switch (currentCups)
            {
                case 0:
                    coffeeAnimator.SetTrigger("Empty");
                    break;
                case 1:
                    coffeeAnimator.SetTrigger("OneCup");
                    break;
                case 2:
                    coffeeAnimator.SetTrigger("TwoCups");
                    break;
            }
        }
    }
}