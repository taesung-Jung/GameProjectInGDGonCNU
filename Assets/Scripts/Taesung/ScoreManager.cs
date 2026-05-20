using UnityEngine;
using System;
using System.Collections; // 코루틴 사용을 위해 필요
using TMPro; // TextMeshPro 사용

public class ScoreManager : MonoBehaviour
{
    public static event Action<int> OnScoreChanged;
    public static ScoreManager Instance { get; private set; }

    public TMP_Text scoreText; // 점수를 표시할 텍스트 컴포넌트

    [Header("Settings")]
    [SerializeField] private int scorePerInterval = 1; // 한 번에 올릴 점수
    [SerializeField] private float intervalTime = 1f;  // 점수가 오르는 시간 간격 (1초)

    private int currentScore = 0;
    private Coroutine scoreCoroutine;

    private void Awake()
    {
        // 싱글톤 패턴
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // 게임 시작 시 이 함수를 호출하여 점수 증가 시작
    public void StartScoring()
    {
        if (scoreCoroutine == null)
        {
            scoreCoroutine = StartCoroutine(AddScoreOverTime());
        }
    }

    // 게임 오버 시 이 함수를 호출하여 점수 증가 중지
    public void StopScoring()
    {
        if (scoreCoroutine != null)
        {
            StopCoroutine(scoreCoroutine);
            scoreCoroutine = null;
        }
    }

    // 설정한 간격마다 점수를 추가하는 코루틴
    private IEnumerator AddScoreOverTime()
    {
        while (true) // 멈출 때까지 무한 반복
        {
            yield return new WaitForSeconds(intervalTime); // 설정한 간격만큼 대기
            AddScore(scorePerInterval); // 정수 점수 가산

            scoreText.text = "" + currentScore; // 점수 텍스트 업데이트
        }
    }

    // 최종 점수를 반환
    public int GetFinalScore() => currentScore;

    // 외부에서 점수를 추가할 때 호출
    public void AddScore(int amount)
    {
        currentScore += amount;
        OnScoreChanged?.Invoke(currentScore);
    }

    // 게임을 리셋할 때 점수를 초기화
    public void ResetScore()
    {
        currentScore = 0;
        OnScoreChanged?.Invoke(currentScore);
    }
}