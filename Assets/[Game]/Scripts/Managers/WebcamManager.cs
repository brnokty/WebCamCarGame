using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebcamManager : MonoBehaviour
{
    #region Singleton

    public static WebcamManager Instance;

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

    private WebCamTexture webcamTexture;

    private void Start()
    {
        // Cihaza bağlı kameraları kontrol et
        if (WebCamTexture.devices.Length > 0)
        {
            var device = WebCamTexture.devices[PlayerPrefsManager.Instance.GetWebCamDevice()];
            webcamTexture = new WebCamTexture(device.name);
            webcamTexture.Play();
            UIManager.Instance.webCamPanel.SetImageTexture(webcamTexture);
            EventManager.Instance.Register(EventTypes.GameStart, _ => webcamTexture.Play());
        }
        else
        {
            Debug.LogError("Webcam bulunamadı!");
        }
    }

    public Texture2D CapturePhoto()
    {
        Texture2D photo = new Texture2D(webcamTexture.width, webcamTexture.height);
        photo.SetPixels(webcamTexture.GetPixels());
        photo.Apply();
        return photo;
    }

    public Texture2D FlipTextureHorizontally(Texture2D texture)
    {
        Color[] pixels = texture.GetPixels();
        Color[] flippedPixels = new Color[pixels.Length];

        int width = texture.width;
        int height = texture.height;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                flippedPixels[y * width + x] = pixels[y * width + (width - 1 - x)];
            }
        }

        texture.SetPixels(flippedPixels);
        texture.Apply();
        return texture;
    }
}