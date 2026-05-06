using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int currentScore = 0;

    public void AddScore(int amount)
    {
        currentScore += amount;
        // 하윤형의 UIManager에 현재 점수 갱신 요청 가능 (이벤트 방식 추천)
    }

    public int GetFinalScore()
    {
        return currentScore;
    }

    public void ResetScore() => currentScore = 0;
}