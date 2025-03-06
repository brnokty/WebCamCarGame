using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : Panel
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    protected override void Start()
    {
        base.Start();
        startButton.onClick.AddListener(OnStartButtonClicked);
        settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
    }

    public void OnStartButtonClicked()
    {
        UIManager.Instance.ShowNameInputPanel();
        SoundManager.Instance.PlaySound(SoundManager.SoundType.ButtonClick);
    }

    public void OnSettingsButtonClicked()
    {
        UIManager.Instance.ShowSettingsPanel();
        SoundManager.Instance.PlaySound(SoundManager.SoundType.ButtonClick);
    }

    public void OnQuitButtonClicked()
    {
        UIManager.Instance.QuitGame();
    }
}