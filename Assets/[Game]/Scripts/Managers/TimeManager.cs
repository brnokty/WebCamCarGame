using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    #region Singleton

    public static TimeManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    [SerializeField] private float startTime;
    [SerializeField] private float raceTime;
    private bool raceActive = false;


    private void Update()
    {
        if (raceActive)
        {
            raceTime = Time.time - startTime;
            UIManager.Instance.inGamePanel.SetRaceTime(raceTime);
        }
    }

    public void StartTimer()
    {
        startTime = Time.time;
        raceActive = true;
    }

    public void StopTimer()
    {
        raceTime = Time.time - startTime;
        raceActive = false;
    }

    public float GetElapsedTime()
    {
        return raceTime;
    }
}