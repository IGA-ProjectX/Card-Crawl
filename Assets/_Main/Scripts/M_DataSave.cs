using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IGDF
{
    public class M_DataSave : MonoBehaviour
    {
        //public void ReadFile(JsonData jsonData)
        //{
        //    // Does the file exist?
        //    if (File.Exists(M_Global.saveFile))
        //    {
        //        // Read the entire file and save its contents.
        //        string fileContents = File.ReadAllText(M_Global.saveFile);

        //        // Deserialize the JSON data 
        //        //  into a pattern matching the GameData class.
        //        jsonData = JsonUtility.FromJson<JsonData>(fileContents);
        //    }
        //    //else
        //    //{
        //    //    string jsonString = JsonUtility.ToJson(jsonData);
        //    //    File.WriteAllText(M_Global.saveFile, jsonString);
        //    //    string fileContents = File.ReadAllText(M_Global.saveFile);
        //    //    jsonData = JsonUtility.FromJson<JsonData>(fileContents);
        //    //}
        //}

        //public void WriteFile(JsonData jsonData)
        //{
        //    // Serialize the object into JSON and save string.
        //    string jsonString = JsonUtility.ToJson(jsonData);

        //    // Write JSON to file.
        //    File.WriteAllText(M_Global.saveFile, jsonString);
        //}
    }
}