using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventRunner : MonoBehaviour
{
    #region Singleton

    public static EventRunner Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion


    public void GameStart()
    {           
        EventManager.Instance.InvokeEvent(EventTypes.GameStart, new EventArgs());
    }


    // public void LevelStart()
    // {
    //     EventManager.Instance.InvokeEvent(EventTypes.LevelStart, new IntArgs(PlayerPrefs.GetInt("Level")));
    // }


    // public void GameFinished()
    // {
    //     EventManager.Instance.InvokeEvent(EventTypes.GameFinish, new EventArgs());
    // }
    
    public void StartRace()
    {
        EventManager.Instance.InvokeEvent(EventTypes.StartRace, new EventArgs());
    }
    
    public void RaceFinish()
    {
        EventManager.Instance.InvokeEvent(EventTypes.RaceFinish, new EventArgs());
    }

    public void SavePhotos()
    {
        EventManager.Instance.InvokeEvent(EventTypes.SavePhotos, new EventArgs());
    }
}