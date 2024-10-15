using UnityEngine;
using UnityEngine.EventSystems;

public class TabsAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Animator tabsAnimator;
    public string highlightedTrigger = "Highlight";
    public string enterTrigger = "Enter";
    public string exitTrigger = "Exit";

    public string scriptToRunName;

    public void OnPointerEnter(PointerEventData eventData)
    {
        tabsAnimator.SetTrigger(highlightedTrigger);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        tabsAnimator.SetTrigger(enterTrigger);

        RunCorrespondingScript();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabsAnimator.SetTrigger(exitTrigger);
    }

    private void RunCorrespondingScript()
    {
        if (!string.IsNullOrEmpty(scriptToRunName))
        {
            MonoBehaviour scriptComponent = GetComponent(scriptToRunName) as MonoBehaviour;
            if (scriptComponent != null)
            {
                scriptComponent.SendMessage("ShowContent", SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                Debug.LogWarning($"Script {scriptToRunName} not found on this GameObject.");
            }
        }
    }
}