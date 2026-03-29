using UnityEngine;

namespace Extensions
{
    public static class FloatExtension 
    {
        public static bool Approximately(float a, float b, float precision) 
            => Mathf.Abs(a - b) <= precision;
    }
}
