using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    public static bool GetBoolData(string dataName)
    {
        return PlayerPrefs.GetInt(dataName, 0) == 1 ? true : false;
    }
    
    public static void SetBoolData(string dataName, bool value)
    {
        PlayerPrefs.SetInt(dataName, value ? 1 : 0);
    }
    
    public static int GetIntData(string dataName)
    {
        return PlayerPrefs.GetInt(dataName, 0);
    }
    
    public static void SetIntData(string dataName, int value)
    {
        PlayerPrefs.SetInt(dataName, value);
    }
    
    public static float GetFloatData(string dataName)
    {
        return PlayerPrefs.GetFloat(dataName, 0F);
    }
    
    public static void SetFloatData(string dataName, float value)
    {
        PlayerPrefs.SetFloat(dataName, value);
    }
    
    public static string GetStringData(string dataName)
    {
        return PlayerPrefs.GetString(dataName, "");
    }
    
    public static void SetStringData(string dataName, string value)
    {
        PlayerPrefs.SetString(dataName, value);
    }
}
