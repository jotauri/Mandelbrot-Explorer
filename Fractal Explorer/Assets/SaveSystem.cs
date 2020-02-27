using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{

    public static List<string> GetFileList()
	{
		List<string> fileList = new List<string>();
		fileList.Add("Load...");
		DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/Saved");
		foreach (FileInfo file in dir.GetFiles("*.prs"))
		{
			fileList.Add(Path.GetFileNameWithoutExtension(file.ToString()));
		}
        return fileList;
    }

    public static bool SavePreset(PresetData data, string name)
    {
        if(name.Length < 1) return false;
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.dataPath + "/Saved/" + name + ".prs";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
        return true;
    }

    public static PresetData LoadPreset(string fileName)
    { 
        string path = Application.dataPath + "/Saved/" + fileName + ".prs";
        if (fileName != "Load..." && File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PresetData data = formatter.Deserialize(stream) as PresetData;
            stream.Close();
            return data;
        }
        else
        {
            PresetData data = new PresetData();
            return data;
        }
    }
}
