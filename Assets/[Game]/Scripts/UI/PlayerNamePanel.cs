using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNamePanel : Panel
{
    public TMP_InputField nameInputField;
    public Button playButton;
    public Button backButton;
    public Panel warningPopUp;

    protected override void Start()
    {
        base.Start();
        playButton.onClick.AddListener(OnPlayButtonClicked);
        backButton.onClick.AddListener(OnBackButtonClicked);
    }

    public void OnPlayButtonClicked()
    {
        if (!string.IsNullOrEmpty(nameInputField.text) && !ScoreManager.Instance.IsPlayerNameTaken(nameInputField.text))
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.PlayClick);
            GameManager.Instance.SetPlayerName(nameInputField.text);
            LevelManager.Instance.OpenGameScene();
        }
        else
        {
            warningPopUp.Appear();
            nameInputField.text = "";
        }

    }
    
    public void OnBackButtonClicked()
    {
        UIManager.Instance.ShowMainPanel();
        SoundManager.Instance.PlaySound(SoundManager.SoundType.ButtonClick);
    }

    public override void WhenDisappearFinished()
    {
        base.WhenDisappearFinished();
        nameInputField.text = "";
    }
}