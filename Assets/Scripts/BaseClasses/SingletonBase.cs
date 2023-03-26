using System;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour
    where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance) return _instance;

            // get instances
            T[] objs = FindObjectsOfType(typeof(T)) as T[];
            

            if (objs.Length == 0)
            {
                // create instance if there is none
                GameObject obj = new GameObject();
                _instance = obj.AddComponent<T>();
            }
            
            else if (objs.Length == 1)
            {
                _instance = objs[0];
            }

            else if (objs.Length > 1)
            {
                _instance = objs[0];
                for ( int i = objs.Length - 1; i > -1; --i)
                {
                    if(_instance == objs[i]) continue;
                    Destroy(objs[i]);
                }
            }

            return _instance;
        }
        
        protected set => _instance = value;
    }

    protected virtual void Awake()
    {
        if (_instance == null) _instance = this as T;
        
        else if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
    }
}


/// <summary>
/// Only use for Managers that will be included in DontDestroyOnLoad.
/// No More Awake on child classes, Use Custom Awake Instead
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class SingletonPersistent<T> : Singleton<T>
    where T : Component
{
    protected override void Awake ()
    {
        base.Awake();
        
        if (Instance.gameObject.scene.buildIndex > -1)
        {
            DontDestroyOnLoad(this);
        }
    }
}
