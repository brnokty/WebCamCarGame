using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Threading.Tasks;

public class BannerManager : MonoBehaviour
{
    #region Singleton

    public static BannerManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    public Banner[] banners;

    private WebcamManager webcamManager;

    private Dictionary<int, List<Texture2D>> bannerPhotos = new Dictionary<int, List<Texture2D>>();

    private Coroutine capturePhotosRoutine;
    private Coroutine saveAllPhotosRoutine;
    private int currentTour;
    public int photosPerBanner = 5;
    private string playerName;

    private void Start()
    {
        playerName = PlayerPrefsManager.Instance.GetPlayerName();
        webcamManager = WebcamManager.Instance;
        EventManager.Instance.Register(EventTypes.StartRace, RaceStart);
        EventManager.Instance.Register(EventTypes.SavePhotos, SaveAllPhotos);
    }

    public void SetLap(int lap)
    {
        currentTour = lap;
    }

    private void RaceStart(EventArgs args)
    {
        capturePhotosRoutine = StartCoroutine(CapturePhotosRoutine());
    }


    private void SaveAllPhotos(EventArgs args)
    {
        if (capturePhotosRoutine != null)
            StopCoroutine(capturePhotosRoutine);
        saveAllPhotosRoutine = StartCoroutine(SaveAllPhotosCoroutine());
    }

    private IEnumerator SaveAllPhotosCoroutine()
    {
        string playerFolderPath = Path.Combine(Application.streamingAssetsPath, "Photos", playerName);
        if (!Directory.Exists(playerFolderPath))
        {
            Directory.CreateDirectory(playerFolderPath);
        }

        var photos = GetAllBannerPhotos();
        for (int i = 0; i < photos.Count; i++)
        {
            string filePath = Path.Combine(playerFolderPath, $"Photo_{i}.png");
            byte[] bytes = photos[i].EncodeToPNG();
            File.WriteAllBytes(filePath, bytes);
            Debug.Log("fotoğraf kaydedildi: " + filePath);
            yield return null;
        }

        Debug.Log("fotoğraflar kaydedildi");
        EventRunner.Instance.RaceFinish();
    }

    private Texture2D IncreaseResolution(Texture2D original, int scaleFactor)
    {
        int newWidth = original.width * scaleFactor;
        int newHeight = original.height * scaleFactor;
        Texture2D highResTexture = new Texture2D(newWidth, newHeight);

        for (int y = 0; y < newHeight; y++)
        {
            for (int x = 0; x < newWidth; x++)
            {
                Color color = original.GetPixelBilinear((float)x / newWidth, (float)y / newHeight);
                highResTexture.SetPixel(x, y, color);
            }
        }

        highResTexture.Apply();
        return highResTexture;
    }

    private IEnumerator CapturePhotosRoutine()
    {
        int bannerIndex = 0;
        int photosTaken = 0;
        Texture2D firstPhoto = null;

        firstPhoto = webcamManager.CapturePhoto();
        if (PlayerPrefsManager.Instance.GetMirror())
        {
            firstPhoto = webcamManager.FlipTextureHorizontally(firstPhoto);
        }

        for (int i = 0; i < banners.Length; i++)
        {
            if (!bannerPhotos.ContainsKey(i))
                bannerPhotos[i] = new List<Texture2D>();

            banners[i].bannerRenderer.material.mainTexture = firstPhoto;
        }

        bannerPhotos[0].Add(firstPhoto);

        yield return new WaitForSeconds(5);

        while (currentTour <= 2 && photosTaken < banners.Length * photosPerBanner)
        {
            Texture2D photo = webcamManager.CapturePhoto();
            if (PlayerPrefsManager.Instance.GetMirror())
            {
                photo = webcamManager.FlipTextureHorizontally(photo);
            }

            Debug.Log("bannerIndex: " + bannerIndex + "bannercount: " + bannerPhotos[bannerIndex].Count);
            if (bannerPhotos[bannerIndex].Count < photosPerBanner)
            {
                bannerPhotos[bannerIndex].Add(photo);
                banners[bannerIndex].bannerRenderer.material.mainTexture = photo;
                photosTaken++;
            }

            bannerIndex = (bannerIndex + 1) % banners.Length;
            yield return new WaitForSeconds(5);
        }
    }

    public List<Texture2D> GetAllBannerPhotos()
    {
        List<Texture2D> orderedPhotos = new List<Texture2D>();

        for (int i = 0; i < banners.Length; i++)
        {
            if (bannerPhotos.ContainsKey(i))
            {
                orderedPhotos.AddRange(bannerPhotos[i]);
            }
        }

        return orderedPhotos;
    }


    private void OnDestroy()
    {
        if (capturePhotosRoutine != null)
            StopCoroutine(capturePhotosRoutine);
        if (saveAllPhotosRoutine != null)
            StopCoroutine(saveAllPhotosRoutine);

        EventManager.Instance.Unregister(EventTypes.StartRace, RaceStart);
        EventManager.Instance.Unregister(EventTypes.SavePhotos, SaveAllPhotos);
    }
}