using UnityEngine;

public class LapManager : MonoBehaviour
{
    public int totalLaps = 3;
    private int currentLap = 0;
    private int checkpointsPassed = 0;
    private int totalCheckpoints;

    public Transform[] checkpoints; // checkpointler
    private bool[] checkpointPassed; // checkpointler geçildimi

    private void Start()
    {
        totalCheckpoints = checkpoints.Length;
        checkpointPassed = new bool[totalCheckpoints];
        BannerManager.Instance.SetLap(currentLap);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            for (int i = 0; i < checkpoints.Length; i++)
            {
                if (other.transform == checkpoints[i] && !checkpointPassed[i])
                {
                    checkpointPassed[i] = true;
                    checkpointsPassed++;

                    // Debug.Log($"Tur {checkpointsPassed}/{totalCheckpoints} tamamlandı!");
                    UIManager.Instance.inGamePanel.SetCheckpoints(checkpointsPassed, totalCheckpoints);
                }
            }
        }
        else if (other.CompareTag("LapTrigger"))
        {
            if (checkpointsPassed >= totalCheckpoints)
            {
                currentLap++;
                checkpointsPassed = 0;
                BannerManager.Instance.SetLap(currentLap);


                for (int j = 0; j < totalCheckpoints; j++)
                    checkpointPassed[j] = false;

                // Debug.Log($"Tur {currentLap}/{totalLaps} tamamlandı!");
                UIManager.Instance.inGamePanel.SetLap(currentLap, totalLaps);
                UIManager.Instance.inGamePanel.SetCheckpoints(checkpointsPassed, totalCheckpoints);

                // Eğer son tur ise yarışı bitir
                if (currentLap >= totalLaps)
                {
                    Debug.Log("Yarış tamamlandı!");
                    // UIManager.Instance.ShowGameEndPanel();
                    SoundManager.Instance.PlaySound(SoundManager.SoundType.Audience);
                    SoundManager.Instance.PlaySound(SoundManager.SoundType.Confetti);
                    SoundManager.Instance.PlaySound(SoundManager.SoundType.LapFinish);
                    UIManager.Instance.SavePhotos();
                    // EventRunner.Instance.SavePhotos();
                    // EventRunner.Instance.RaceFinish();
                }
                else
                {
                    SoundManager.Instance.PlaySound(SoundManager.SoundType.LapPass);
                }

                // if (currentLap < 2)
                // {
                //     BannerManager.Instance.TakePhotoForBanner();
                // }
            }
        }
    }
}