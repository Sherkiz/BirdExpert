using UnityEngine;

public static  class GOUtilities
{
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        if (!gameObject.TryGetComponent<T>(out T component)) component = gameObject.AddComponent<T>();
        return component;
    }
}
