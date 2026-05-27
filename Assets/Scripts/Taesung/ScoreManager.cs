using UnityEngine;
using System;
using System.Collections;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    // 점수 관리 및 점수 변경 이벤트를 담당하는 싱글톤 클래스
    public static event Action<int> OnScoreChanged;
    public static ScoreManager Instance { get; private set; }

    public TMP_Text scoreText; // 점수 출력 Text 창

    [Header("Settings")]
    [SerializeField] private int scorePerInterval = 1; // 일정 시간마다 추가되는 점수 (1점)
    [SerializeField] private float intervalTime = 1f;  // 점수 추가 간격 (1초)

    private int currentScore = 0; // 현재 점수
    private Coroutine scoreCoroutine; // 점수 추가 코루틴

    private void Awake()
    {
        // 싱글톤 관리
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // 일정 시간마다 점수를 추가하는 코루틴 시작
    public void StartScoring()
    {
        if (scoreCoroutine == null)
        {
            scoreCoroutine = StartCoroutine(AddScoreOverTime());
        }
    }

    // 점수 추가 코루틴 중지
    public void StopScoring()
    {
        if (scoreCoroutine != null)
        {
            StopCoroutine(scoreCoroutine);
            scoreCoroutine = null;
        }
    }

    // 일정 시간마다 점수를 추가하는 코루틴
    private IEnumerator AddScoreOverTime()
    {
        // 무한 루프를 돌면서 일정 시간마다 점수 추가
        while (true)
        {
            yield return new WaitForSeconds(intervalTime); // 일정 시간 대기
            AddScore(scorePerInterval); // 점수 추가

            scoreText.text = "" + currentScore; // 점수 텍스트 업데이트
        }
    }

    // 최종 점수 반환
    public int GetFinalScore() => currentScore;

    // 점수 추가 메서드
    public void AddScore(int amount)
    {
        currentScore += amount;
        OnScoreChanged?.Invoke(currentScore);
    }

    // 점수 초기화 메서드
    public void ResetScore()
    {
        currentScore = 0;
        OnScoreChanged?.Invoke(currentScore);
    }
}