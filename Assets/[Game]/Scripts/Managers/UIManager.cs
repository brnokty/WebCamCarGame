using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Singleton

    public static UIManager Instance;

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

    public MainPanel mainPanel;
    public SettingsPanel settingsPanel;
    public PlayerNamePanel playerNamePanel;
    public InGamePanel inGamePanel;
    public RaceEndPanel raceEndPanel;
    public WebCamPanel webCamPanel;
    public LoadingPanel loadingPanel;

    private void Start()
    {
        EventManager.Instance.Register(EventTypes.GameStart, inGamePanel.StartCountdown);
    }

    public void ShowNameInputPanel()
    {
        playerNamePanel.Appear();

        //disable other panels
        mainPanel.Disappear();
        settingsPanel.Disappear();
        raceEndPanel.Disappear();
        inGamePanel.Disappear();
        loadingPanel.Disappear();
    }

    public void ShowMainPanel()
    {
        mainPanel.Appear();

        //disable other panels
        playerNamePanel.Disappear();
        settingsPanel.Disappear();
        raceEndPanel.Disappear();
        inGamePanel.Disappear();
        loadingPanel.Disappear();
    }

    public void ShowSettingsPanel()
    {
        settingsPanel.Appear();

        //disable other panels
        mainPanel.Disappear();
        playerNamePanel.Disappear();
        raceEndPanel.Disappear();
        inGamePanel.Disappear();
        loadingPanel.Disappear();
    }

    public void ShowInGamePanel()
    {
        inGamePanel.Appear();

        //disable other panels
        mainPanel.Disappear();
        playerNamePanel.Disappear();
        raceEndPanel.Disappear();
        settingsPanel.Disappear();
        loadingPanel.Disappear();
    }

    public void ShowGameEndPanel()
    {
        raceEndPanel.Appear();

        //disable other panels
        mainPanel.Disappear();
        playerNamePanel.Disappear();
        inGamePanel.Disappear();
        settingsPanel.Disappear();
        loadingPanel.Disappear();
    }

    public void ShowLoadingPanel()
    {
        loadingPanel.Appear();

        //disable other panels
        mainPanel.Disappear();
        playerNamePanel.Disappear();
        inGamePanel.Disappear();
        settingsPanel.Disappear();
        raceEndPanel.Disappear();
    }

    public void SavePhotos()
    {
        ShowLoadingPanel();
    }

    //hide all panels
    public void HideAllPanels()
    {
        mainPanel.Disappear();
        playerNamePanel.Disappear();
        inGamePanel.Disappear();
        raceEndPanel.Disappear();
        settingsPanel.Disappear();
        loadingPanel.Disappear();
    }

    //Open web cam panel

    public void OpenWebCamPanel()
    {
        webCamPanel.Appear();
    }

    //Close
    public void CloseWebCamPanel()
    {
        webCamPanel.Disappear();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}