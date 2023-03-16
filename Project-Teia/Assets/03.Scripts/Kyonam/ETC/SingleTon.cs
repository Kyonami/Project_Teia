using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTon<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    private static bool firstInstance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(T)) as T;
                firstInstance = true;
                if (instance == null)
                    Debug.Log("There is No Active '" + typeof(T) + "'in this Scene");
            }
            else
            {
                firstInstance = false;
            }
            return instance;
        }
    }

    public static bool FirstInstance { get { return firstInstance; } }
}
