using System;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance = null;
    public static T Get
    {
        get
        {
            return instance;
        }
        protected set
        {
            instance = value;
        }
    }

    public static Action OnSingletonInitialized = delegate { };

    protected virtual void PostAwake() { }
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        Get = this as T;
        OnSingletonInitialized();
        PostAwake();
    }

    protected virtual void PreDestroy() { }
    private void OnDestroy()
    {
        if (Get != this) return;

        PreDestroy();
        Get = null;
    }
}