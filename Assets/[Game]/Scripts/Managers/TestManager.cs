using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    #region Singleton

    public static TestManager Instance;

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

    private bool isTurnOn;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UIManager.Instance.SavePhotos();
            //EventRunner.Instance.SavePhotos();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isTurnOn = !isTurnOn;
            if (isTurnOn)
                UIManager.Instance.OpenWebCamPanel();
            else
                UIManager.Instance.CloseWebCamPanel();
        }
    }
}