using System;
using UnityEngine;

public class StorageManager 
{
    public IBaseDataSaver BaseDataSaver { get; private set; } = new BaseDataSaver();
    public JsonSaver JsonSaver { get; private set; }
    public static event Action onCoinsAmountChanged;

    public void Init()
    {
        JsonSaver = new JsonSaver(BaseDataSaver);
    }

    public int CoinsAmount
    {
        get => GetCollectable(eCollectable.Meat);
        set
        {
            onCoinsAmountChanged?.Invoke();
            SetCollectable(eCollectable.Meat, value);
        }
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