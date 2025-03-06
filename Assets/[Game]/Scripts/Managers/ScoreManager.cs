using UnityEngine;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class ScoreEntry
{
    public string playerName;
    public float raceTime;
}

[System.Serializable]
public class ScoreData
{
    public List<ScoreEntry> scores = new List<ScoreEntry>();
}

public class ScoreManager : MonoBehaviour
{
    #region Singleton

    public static ScoreManager Instance;

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
            return;
        }

        savePath = Path.Combine(Application.streamingAssetsPath, "scores.json");
        LoadScores();
    }

    #endregion

    private string savePath;

    public List<ScoreEntry> allScores = new List<ScoreEntry>(); // Tüm skorlar
    public List<ScoreEntry> top3Scores = new List<ScoreEntry>(); // İlk 3 için ayrı liste

    public void SaveScore(string playerName, float raceTime)
    {
        ScoreEntry newEntry = new ScoreEntry { playerName = playerName, raceTime = raceTime };
        allScores.Add(newEntry);


        allScores.Sort((a, b) => a.raceTime.CompareTo(b.raceTime));

        // JSON olarak tüm skorları kaydet
        ScoreData scoreData = new ScoreData { scores = allScores };
        string json = JsonUtility.ToJson(scoreData, true);
        File.WriteAllText(savePath, json);

        // top 3ü güncelle
        UpdateTop3Scores();

        Debug.Log($"Skor kaydedildi: {playerName} - {raceTime:F2} sn");
    }

    private void UpdateTop3Scores()
    {
        top3Scores.Clear();
        int count = Mathf.Min(3, allScores.Count);
        for (int i = 0; i < count; i++)
        {
            top3Scores.Add(allScores[i]);
        }
    }

    public void LoadScores()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);

            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    ScoreData scoreData = JsonUtility.FromJson<ScoreData>(json);

                    if (scoreData != null && scoreData.scores != null)
                    {
                        allScores = scoreData.scores;
                    }
                    else
                    {
                        allScores = new List<ScoreEntry>();
                    }
                }
                catch
                {
                    allScores = new List<ScoreEntry>();
                }
            }
            else
            {
                allScores = new List<ScoreEntry>();
            }
        }
        else
        {
            allScores = new List<ScoreEntry>();
        }

        UpdateTop3Scores();
    }


    public bool IsPlayerNameTaken(string playerName)
    {
        foreach (var entry in allScores)
        {
            if (entry.playerName == playerName)
            {
                return true;
            }
        }

        return false;
    }
}