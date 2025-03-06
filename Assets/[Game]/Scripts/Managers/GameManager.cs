using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager Instance;

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

    public string playerName { get; private set; } // Oyuncu adı

    private void Start()
    {
        EventManager.Instance.Register(EventTypes.StartRace, StartRace);
        EventManager.Instance.Register(EventTypes.SavePhotos, EndRace);
    }

    public void SetPlayerName(string name)
    {
        playerName = name;
        PlayerPrefsManager.Instance.SetPlayerName(playerName);
    }

    public void StartRace(EventArgs args)
    {
        TimeManager.Instance.StartTimer(); // timer i başlat
    }

    public void EndRace(EventArgs args)
    {
        TimeManager.Instance.StopTimer();

        float finalTime = TimeManager.Instance.GetElapsedTime();
        Debug.Log($"Yarış Bitti! playerName: {playerName}, time: {finalTime:F2} sn");
        ScoreManager.Instance.SaveScore(playerName, finalTime);
        UIManager.Instance.ShowGameEndPanel();
    }
}