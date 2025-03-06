using System;
using System.Collections.Generic;
using UnityEngine;

public class MainThreadDispatcher : MonoBehaviour
{
    private static readonly Queue<Action> _executionQueue = new Queue<Action>();
    private static MainThreadDispatcher _instance;

    public static MainThreadDispatcher Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("MainThreadDispatcher");
                _instance = obj.AddComponent<MainThreadDispatcher>();
                DontDestroyOnLoad(obj);
            }

            return _instance;
        }
    }

    private void Update()
    {
        while (_executionQueue.Count > 0)
        {
            _executionQueue.Dequeue()?.Invoke();
        }
    }

    public void Enqueue(Action action)
    {
        lock (_executionQueue)
        {
            _executionQueue.Enqueue(action);
        }
    }
}