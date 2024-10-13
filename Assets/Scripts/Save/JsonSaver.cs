using UnityEngine;

public class JsonSaver
{
    private readonly IBaseDataSaver m_BaseDataSaver;

    public JsonSaver(IBaseDataSaver baseDataSaver)
    {
        m_BaseDataSaver = baseDataSaver;
    }

    public bool HasSave<T>(out T saveData, string id) where T : class
    {
        if (m_BaseDataSaver.TryGetString(id, out string content))
        {
            saveData = JsonUtility.FromJson<T>(content);
            return true;
        }

        saveData = null;
        return false;
    }

    public void Save<T>(T saveData, string id) where T : class
    {
        string jsonString = JsonUtility.ToJson(saveData);

        m_BaseDataSaver.SetString(id, jsonString);
    }
}