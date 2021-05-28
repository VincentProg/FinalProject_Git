using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveSystem
{
    private static int isPlayingForFirstTime = PlayerPrefs.GetInt("ftp", 1);
    public static void SaveOptions(AudioManager audioManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/option.yeet";
        FileStream stream = new FileStream(path, FileMode.Create);
        OptionsData data = new OptionsData(audioManager);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static OptionsData LoadOptions()
    {
        if(isPlayingForFirstTime == 1)
        {
            SaveOptions(AudioManager.instance);
            Debug.Log("playing for the first time");
            PlayerPrefs.SetInt("ftp", 0);

        }
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
            Debug.Log(path.ToString());
            Debug.LogError("ERROR - Options Data File Was Not Found in "+ path);
            return null;
        }
    }
}
