using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveSystem
{

    public static void SaveOptions(AudioManager audioManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/option.yeet";
        Debug.Log(path);
        FileStream stream = new FileStream(path, FileMode.Create);
        OptionsData data = new OptionsData(audioManager);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static OptionsData LoadOptions()
    {
        string path = Application.persistentDataPath + "/option.yeet";
        
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            OptionsData data = formatter.Deserialize(stream) as OptionsData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("ERROR - Options Data File Was Not Found in "+ path);
            return null;
        }
    }
}
