using UnityEngine;

public static class LayerExtension
{
    public static void ChangeMaskB(this GameObject gameObject, LayerMask layer) 
        => gameObject.layer = layer.value >> 1;

    public static void ChangeMask(this GameObject gameObject, LayerMask layer) 
        => gameObject.layer = (int)Mathf.Log(layer.value, 2);
    
    public static void ToSingleMaskF(this GameObject gameObject, LayerMask layer)
    {
        int layerMask = ToSingleLayer(layer);
        gameObject.layer = layerMask;
    }
    
    public static void ToSingleMaskL(this GameObject gameObject, LayerMask layer)
    {
        int layerMask = LayerMaskToLayer(layer);
        gameObject.layer = layerMask;
    }
    
    public static int ToIntMask(this LayerMask layer) 
        =>  (int)Mathf.Log(layer.value, 2);
    
    public static int ToSingleLayer(this LayerMask mask)
    {
        int value = mask.value;
        if (value == 0) return 0;  // Early out
        for (int l = 1; l < 32; l++)
            if ((value & (1 << l)) != 0) return l;  // Bitwise
        return -1;  // This line won't ever be reached but the compiler needs it
    }
    
    public static int LayerMaskToLayer(LayerMask layerMask) {
        int layerNumber = 0;
        int layer = layerMask.value;
        while(layer > 0) {
            layer = layer >> 1;
            layerNumber++;
        }
        return layerNumber - 1;
    }
}