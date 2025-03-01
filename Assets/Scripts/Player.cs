using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public int counter = 0;

    public void SavePlayer()
    {
        SaveStructure.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveStructure.LoadPlayer();
        if (data != null)
        {
            counter = data.counterValue;
            counterText.text = counter.ToString();
            Debug.Log("Data loaded successfully");
        }
        else
        {
            Debug.Log("No save data found - starting with default values");
            counter = 0;
            counterText.text = counter.ToString();
        }
    }

    public TextMeshProUGUI counterText;

    public void IncreaseCounter()
    {
        counter++;
        counterText.text = counter.ToString();
    }
}