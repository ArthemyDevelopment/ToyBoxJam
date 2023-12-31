using System;

using System.Collections.Generic;
using UnityEngine;

public static class HitboxRecognitionSystem
{
    private static Dictionary<Collider2D, Action<float>> _DamageDic= new Dictionary<Collider2D, Action<float>>();

    public static void AddObject(Collider2D col, Action<float> method)
    {
        _DamageDic.Add(col, method);
    }

    public static void RemoveObject(Collider2D col)
    {
        _DamageDic.Remove(col);
    }

    public static Action<float> GetEvent(Collider2D col)
    {
        if(_DamageDic.ContainsKey(col))
            return _DamageDic[col];
        else
        {
            Debug.LogError("Collider not found in dictionary: Please check the awake method of the target and verify its init", col.gameObject);
            return null;
        }
        
    }
    
    
    
}
