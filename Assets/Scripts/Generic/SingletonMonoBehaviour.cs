using System;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private bool isLoadPersistent;

    public static T Get { get; protected set; }
    public static Action OnSingletonInitialized = delegate { };

    protected virtual void Awake()
    {
        if (Get != null)
        {
            Destroy(this);
        }
        else
        {
            Get = this as T;
            if (isLoadPersistent) DontDestroyOnLoad(Get);
            OnSingletonInitialized();
        }
    }

    protected virtual void OnDestroy()
    {
        if (Get == this) Get = null;
    }
}