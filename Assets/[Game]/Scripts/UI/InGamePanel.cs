using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class InGamePanel : Panel
{
    [SerializeField] private TMP_Text gearText;
    [SerializeField] private TMP_Text checkpointsText;
    [SerializeField] private TMP_Text lapText;
    [SerializeField] private TMP_Text raceTimeText;
    [SerializeField] private TMP_Text speedText;

    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private float scaleDuration = 0.5f;
    [SerializeField] private float delayBetweenNumbers = 0.5f;

    private int countdownValue = 3;

    protected override void Start()
    {
        base.Start();
        // EventManager.Instance.Register(EventTypes.GameStart, StartCountdown);
    }
    //
    // public void OnPauseButtonClicked()
    // {
    //     GameManager.Instance.PauseGame();
    // }
    //
    // public void OnResumeButtonClicked()
    // {
    //     GameManager.Instance.ResumeGame();
    // }
    //
    // public void OnRestartButtonClicked()
    // {
    //     GameManager.Instance.RestartGame();
    // }
    //
    // public void OnQuitButtonClicked()
    // {
    //     GameManager.Instance.QuitGame();
    // }

    public void SetGear(int gear)
    {
        gearText.text = gear == 1 ? "D" : "R";
    }

    public void SetCheckpoints(int current, int total)
    {
        checkpointsText.text = $"{current}/{total}";
    }

    public void SetLap(int current, int total)
    {
        lapText.text = $"{current}/{total}";
    }

    public void SetRaceTime(float time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);

        string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D3}",
            timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);

        raceTimeText.text = formattedTime;
    }

    public void SetSpeed(float speed)
    {
        speedText.text = $"{speed:000}";
    }

    // Start UI
    public void StartCountdown(EventArgs args)
    {
        countdownValue = 3;
        if (countdownText.gameObject != null)
            countdownText.gameObject.SetActive(true);
        UpdateCountdown();
    }

    private void UpdateCountdown()
    {
        if (countdownValue < 0)
        {
            countdownText.gameObject.SetActive(false);
            return;
        }

        countdownText.text = countdownValue.ToString();
        countdownText.transform.localScale = Vector3.zero;

        countdownText.transform.DOScale(Vector3.one, scaleDuration).SetEase(Ease.OutBounce)
            .OnComplete(() =>
            {
                if (countdownValue == 0)
                {
                    SoundManager.Instance.PlaySound(SoundManager.SoundType.Biiip);
                    GameStarted();
                }
                else
                {
                    SoundManager.Instance.PlaySound(SoundManager.SoundType.Bip);
                }

                countdownText.transform.DOScale(Vector3.zero, scaleDuration).SetEase(Ease.InBack)
                    .OnComplete(() =>
                    {
                        countdownValue--;
                        Invoke(nameof(UpdateCountdown), delayBetweenNumbers);
                    });
            });
    }

    private void GameStarted()
    {
        EventRunner.Instance.StartRace();
    }

    //reset all Texts
    public void ResetAllTexts()
    {
        gearText.text = "D";
        checkpointsText.text = "0/7";
        lapText.text = "0/3";
        raceTimeText.text = "00:00:000";
        speedText.text = "000";
    }

    public override void WhenDisappearFinished()
    {
        base.WhenDisappearFinished();
        ResetAllTexts();
    }
}