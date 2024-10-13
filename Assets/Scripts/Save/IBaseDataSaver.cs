public interface IBaseDataSaver
{
    public void SetBool(string key, bool value);
    public bool GetBool(string key, bool defaultValue = false);
    public bool HasKay(string key);
    
    public void SetInt(string key, int value);
    public int GetInt(string key, int defaultValue);
    
    public void SetFloat(string key, float value);
    public float GetFloat(string key, float defaultValue);
    
    public void SetString(string key, string value);
    public bool TryGetString(string key, out string value);
}