using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : Panel
{
    [SerializeField] private Button backButton;
    [SerializeField] TMP_Dropdown webCamDevicesDropdown;
    [SerializeField] private Toggle soundToggle;
    [SerializeField] private Toggle mirrorToggle;

    private WebCamDevice[] devices;

    protected override void Start()
    {
        base.Start();
        backButton.onClick.AddListener(OnBackButtonClicked);
        soundToggle.onValueChanged.AddListener(OnSoundToggleValueChanged);
        mirrorToggle.onValueChanged.AddListener(OnMirrorToggleValueChanged);
        webCamDevicesDropdown.onValueChanged.AddListener(OnWebCamDevicesDropdownValueChanged);


        WebCamDevicesDropdownSetup();
        soundToggle.isOn = PlayerPrefsManager.Instance.GetSound();
        AudioListener.volume = soundToggle.isOn ? 1 : 0;
        mirrorToggle.isOn = PlayerPrefsManager.Instance.GetMirror();
    }

    private void WebCamDevicesDropdownSetup()
    {
        devices = WebCamTexture.devices;
        if (devices.Length == 0)
        {
            Debug.LogError("Kamera bulunamadı!");
            return;
        }

        webCamDevicesDropdown.ClearOptions();
        foreach (var device in devices)
        {
            webCamDevicesDropdown.options.Add(new TMP_Dropdown.OptionData(device.name));
        }

        webCamDevicesDropdown.value = PlayerPrefsManager.Instance.GetWebCamDevice();
    }

    public void OnBackButtonClicked()
    {
        SoundManager.Instance.PlaySound(SoundManager.SoundType.ButtonClick);
        UIManager.Instance.ShowMainPanel();
    }

    public void OnSoundToggleValueChanged(bool value)
    {
        SoundManager.Instance.PlaySound(SoundManager.SoundType.ButtonClick2);
        AudioListener.volume = value ? 1 : 0;
        PlayerPrefsManager.Instance.SetSound(value);
    }

    public void OnMirrorToggleValueChanged(bool value)
    {
        SoundManager.Instance.PlaySound(SoundManager.SoundType.ButtonClick2);
        PlayerPrefsManager.Instance.SetMirror(value);
    }

    public void OnWebCamDevicesDropdownValueChanged(int value)
    {
        if (canvasGroup.interactable)
            SoundManager.Instance.PlaySound(SoundManager.SoundType.ButtonClick2);
        PlayerPrefsManager.Instance.SetWebCamDevice(value);
    }
}