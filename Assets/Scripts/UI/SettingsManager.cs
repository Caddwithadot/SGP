/*******************************************************************************
Author: Jared
State: Currently working on
Description:
 Contains the save and load settings functions to store settings locally.
*******************************************************************************/

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    private string SaveFileName = "\\Scripts/UI/Settings.txt";

    public SettingsSaveData CurrentSettings;

    private void Start()
    {
        //Move this to the pause manager when one is made
        //LoadSettings();
    }

    public void SaveSettings()
    {
        SettingsManagerSaveData data = new SettingsManagerSaveData();

        data.Settings.Add(CurrentSettings);

        string text = JsonUtility.ToJson(data);
        File.WriteAllText(Application.dataPath + SaveFileName, text);
    }

    public void LoadSettings()
    {
        if (!File.Exists(Application.dataPath + SaveFileName))
            return;

        string text = File.ReadAllText(Application.dataPath + SaveFileName);
        SettingsManagerSaveData data = JsonUtility.FromJson<SettingsManagerSaveData>(text);

        CurrentSettings = data.Settings[0];
    }
}

public class SettingsManagerSaveData
{
    public List<SettingsSaveData> Settings = new List<SettingsSaveData>();
}

[System.Serializable]
public class SettingsSaveData
{
    public bool FullScreen = true;
    public int Resolution = 0;
    public float MasterVolume = 20f;
    public float BGMVolume = 20f;
    public float SFXVolume = 20f;
    public float MouseXSens = 100f;
    public float MouseYSens = 100f;
}