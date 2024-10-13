using Extensions;
using UnityEngine;

public class BaseDataSaver : IBaseDataSaver
{
    public bool HasKay(string key) => PlayerPrefs.HasKey(key);

    #region Bool
    public void SetBool(string key, bool value) => PlayerPrefsBool.SetBool(key, value);
    public bool GetBool(string key, bool defaultValue = false) => PlayerPrefsBool.GetBool(key, defaultValue);
    #endregion

    #region Int
    public void SetInt(string key, int value) => PlayerPrefs.SetInt(key, value);
    public int GetInt(string key, int defaultValue) => PlayerPrefs.GetInt(key, defaultValue);
    #endregion

    #region Float
    public void SetFloat(string key, float value) => PlayerPrefs.SetFloat(key, value);
    public float GetFloat(string key, float defaultValue) => PlayerPrefs.GetFloat(key, defaultValue);
    #endregion
    
    #region String
    public void SetString(string key, string value) => PlayerPrefs.SetString(key, value);

    public bool TryGetString(string key, out string value)
    {
        var hasValue = PlayerPrefs.HasKey(key);
        value = hasValue ? PlayerPrefs.GetString(key) : "";

        return hasValue;
    }
    #endregion
}