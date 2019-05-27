using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ExtensionMethods {
 
    public static List<Transform> FindChildren(this Transform transform, string name)
    {
        return transform.GetComponentsInChildren<Transform>().Where(t => t.name == name).ToList();
    }
}