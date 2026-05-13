using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    private const string BEST_SCORE_KEY = "BestScore";

    public bool IsBestScore(int newScore)
    {
        int oldBest = PlayerPrefs.GetInt(BEST_SCORE_KEY, 0);
        return newScore > oldBest;
    }

    public void SaveScoreLocally(int score)
    {
        PlayerPrefs.SetInt(BEST_SCORE_KEY, score);
        PlayerPrefs.Save();
    }

    public int GetLocalBestScore() => PlayerPrefs.GetInt(BEST_SCORE_KEY, 0);
}
