using UnityEngine;
using System.Collections;

public class MapManager : MonoBehaviour
{
    [Header("UI 설정")]
    public GameObject warningText;

    [Header("속도 설정")]
    public float normalSpeed = 7.0f; // 평소 달리기 속도
    public float slowSpeed = 4.0f;   // 파괴벽 모드일 때 느린 속도

    [Header("모드 시작 조건")]
    public float timeToWallMode = 20.0f;
    private float timer = 0f;
    private bool isWallModeStarted = false;

    // 외부 참조
    public PlatformSpawner platformSpawner;

    void Start()
    {
        // 게임 시작 시 평소 속도를 스포너에 전달
        platformSpawner.currentSpeed = normalSpeed;
    }

    void Update()
    {
        if (!isWallModeStarted)
        {
            timer += Time.deltaTime;
            if (timer >= timeToWallMode)
            {
                StartWallMode();
            }
        }
    }

    void StartWallMode()
    {
        isWallModeStarted = true;

        StartCoroutine(ShowWarningRoutine());

        //  스포너에게 느린 속도 지시 및 파괴벽 모드 트리거
        platformSpawner.TriggerWallMode(slowSpeed);
    }

    //  (신규) 파괴벽이 부서지면 이 함수가 호출됩니다.
    public void EndWallMode()
    {
        Debug.Log("MapManager: 벽 파괴 확인! 평소 모드로 복귀합니다.");

        // 스포너에게 평소 속도로 복귀 명령 및 파괴벽 모드 종료
        platformSpawner.TriggerNormalMode(normalSpeed);

        // 타이머 리셋 및 스위치 꺼서 다시 시간이 흐르면 벽 모드가 되게 함
        timer = 0f;
        isWallModeStarted = false;
    }

    IEnumerator ShowWarningRoutine()
    {
        for (int i = 0; i < 3; i++)
        {
            warningText.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            warningText.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }
}