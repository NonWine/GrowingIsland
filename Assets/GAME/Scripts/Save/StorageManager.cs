using System;
using UnityEngine;

public class StorageManager 
{
    public IBaseDataSaver BaseDataSaver { get; private set; } = new BaseDataSaver();
    public JsonSaver JsonSaver { get; private set; }
    public static event Action onCoinsAmountChanged;

    public StorageManager()
    {
        JsonSaver = new JsonSaver(BaseDataSaver);
    }
    

    public int GetCollectable(eCollectable eCollectable)
    {
        return PlayerPrefs.GetInt(eCollectable.ToString(), 0);
    }

    public void SetCollectable(eCollectable eCollectable, int amount)
    {
        PlayerPrefs.SetInt(eCollectable.ToString(), amount);
    }
}