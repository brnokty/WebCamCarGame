using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RaceEndPanel : Panel
{
    public TMP_Text playerNameText;
    public TMP_Text timeText;
    public List<Line> LeaderboardLines = new List<Line>();
    public Button mainMenuButton;

    public Image[] bannerDisplaySlots; // Canvas üzerindeki Image objeleri
    private BannerManager bannerManager;
    public GameObject imagePrefab;
    public Transform content;

    private List<GameObject> contentObjects = new List<GameObject>();


    protected override void Start()
    {
        base.Start();
        mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
        EventManager.Instance.Register(EventTypes.SavePhotos, SetPanel);
    }


    private void SetPanel(EventArgs args)
    {
        float finalTime = TimeManager.Instance.GetElapsedTime();
        TimeSpan timeSpan = TimeSpan.FromSeconds(finalTime);

        string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D3}",
            timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);

        playerNameText.text = GameManager.Instance.playerName;
        timeText.text = formattedTime;

        ShowLeaderboard();
    }

    public void ShowLeaderboard()
    {
        var top3 = ScoreManager.Instance.top3Scores;
        for (int i = 0; i < top3.Count; i++)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(top3[i].raceTime);

            string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D3}",
                timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
            if (LeaderboardLines[i] != null && LeaderboardLines[i].gameObject != null)
                LeaderboardLines[i].SetLine((i + 1).ToString(), top3[i].playerName, formattedTime);
        }
    }

    public void OnMainMenuButtonClicked()
    {
        SoundManager.Instance.PlaySound(SoundManager.SoundType.ButtonClick);
        LevelManager.Instance.OpenMainMenuScene();
        UIManager.Instance.ShowMainPanel();
    }


    public override void Appear()
    {
        base.Appear();
        bannerManager = BannerManager.Instance;
        SetPanel(null);
        DisplayPhotos();
    }

    // public override void WhenAppearFinished()
    // {
    //     base.WhenAppearFinished();
    //     
    //     //DisplayPhotos();
    //     SetPanel(null);
    // }

    public override void WhenDisappearFinished()
    {
        base.WhenDisappearFinished();
        ResetPanel();
    }

    private void DisplayPhotos()
    {
        var photos = BannerManager.Instance.GetAllBannerPhotos();

        for (int i = 0; i < photos.Count; i++)
        {
            var image = Instantiate(imagePrefab, content).GetComponent<Image>();
            contentObjects.Add(image.gameObject);
            image.sprite = Sprite.Create(photos[i], new Rect(0, 0, photos[i].width, photos[i].height),
                Vector2.one * 0.5f);
        }
    }

    public void ResetPanel()
    {
        for (int i = 0; i < contentObjects.Count; i++)
        {
            Destroy(contentObjects[i]);
        }

        contentObjects.Clear();
    }
}