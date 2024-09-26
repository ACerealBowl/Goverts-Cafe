using UnityEngine;
using UnityEngine.EventSystems;

public class CashAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Animator cashRegisterAnimator;

    public string highlightedTrigger = "Highlight";
    public string enterTrigger = "Enter";
    public string exitTrigger = "Exit";

    public void OnPointerEnter(PointerEventData eventData)
    {
        cashRegisterAnimator.SetTrigger(highlightedTrigger);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        cashRegisterAnimator.SetTrigger(enterTrigger);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cashRegisterAnimator.SetTrigger(exitTrigger);
    }
}
