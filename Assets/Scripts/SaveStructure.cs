using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveStructure
{
    public static void SavePlayer(Player player) //save
    {
        BinaryFormatter formatter = new BinaryFormatter(); 
        string path = Application.persistentDataPath + "/player.govert"; //path
        FileStream stream = new FileStream(path, FileMode.Create); //notice create

        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);
        stream.Close();
        Debug.Log("saved" + path);
    }

    public static PlayerData LoadPlayer() //load
    {
        string path = Application.persistentDataPath + "/player.govert";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open); //notice open, its like ifstream honestly.
            PlayerData data = formatter.Deserialize(stream) as PlayerData; 
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("cucked the Save file cookoo" + path);
            return null;
        }
    }
}