using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region Singleton

    public static LevelManager Instance;

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

    //open gameScene
    public void OpenGameScene()
    {
        UIManager.Instance.HideAllPanels();
        SceneManager.LoadScene("GameScene");
        UIManager.Instance.ShowInGamePanel();
    }

    //open mainMenuScene
    public void OpenMainMenuScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}