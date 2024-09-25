using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFrame : MonoBehaviour
{
    public void Story()
    {
        Application.Quit();
    }
    public void Endless()
    {
        SceneManager.LoadSceneAsync("Endless");
    }
    public void Race()
    {
        Application.Quit();
    }
}