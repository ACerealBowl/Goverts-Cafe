using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public Animator menuAnim;
    public void Open()
    {
        menuAnim.SetTrigger("Open");
    }

    public void Close()
    {
        menuAnim.SetTrigger("Close");
    }
}
