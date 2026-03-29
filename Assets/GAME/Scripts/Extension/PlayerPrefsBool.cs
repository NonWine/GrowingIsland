using UnityEngine;

namespace Extensions
{
    public static class PlayerPrefsBool
    {
        public static bool GetBool(string key, bool Default = false) 
            => PlayerPrefs.HasKey(key) ? PlayerPrefs.GetInt(key) == 1 : Default;

        public static void SetBool(string key, bool state) 
            => PlayerPrefs.SetInt(key, state ? 1 : 0);

        public static bool LoadBool(this string key, bool Default = false) 
            => PlayerPrefs.HasKey(key) ? PlayerPrefs.GetInt(key) == 1 : Default;

        public static bool HasKey(this string key) 
            => PlayerPrefs.HasKey(key);

        public static void SaveBool(this bool state, string key) 
            => PlayerPrefs.SetInt(key, state ? 1 : 0);
    }
}


