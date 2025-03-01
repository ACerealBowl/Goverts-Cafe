using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int counterValue = 0;

    public PlayerData(Player player)
    {
        counterValue = player.counter;
    }
}