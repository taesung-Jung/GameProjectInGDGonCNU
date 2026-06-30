using UnityEngine;
using System.Collections;

public class MapManager : MonoBehaviour
{
    [Header("UI 설정")]
    public GameObject warningText;

    [Header("속도 설정")]
    public float normalSpeed = 7.0f;
    public float slowSpeed = 4.0f;

    [Header("모드 설정")]
    public float minModeTime = 8.0f;  // 최소 모드 유지 시간
    public float maxModeTime = 15.0f; // 최대 모드 유지 시간

    private float timer = 0f;
    private float nextModeTime;
    private int currentMode = 0; // 0: Normal, 1: Wall, 2: Flight

    public PlatformSpawner platformSpawner;
    public PlayerController player;

    void Start()
    {
        timer = 0f;
        nextModeTime = 9999f; // 테스트 중에 모드가 안 바뀌도록 아주 길게 설정

        // 👇 여기서 테스트하고 싶은 모드 딱 하나만 주석을 풀고 실행하세요! 👇

        // 1. 일반 장애물(발판, 기본 가시 등)만 주구장창 잘 나오는지 테스트할 때
        platformSpawner.TriggerNormalMode(normalSpeed);

        // 2. 비행 장애물(위아래 기둥)만 쭈욱 잘 나오는지 테스트할 때 (이걸 테스트할 땐 위 1번을 주석 처리하세요)
        //platformSpawner.TriggerFlightMode(normalSpeed);

        // 3. 파괴벽만 잘 나오는지 테스트할 때 (이걸 테스트할 땐 위 1번을 주석 처리하세요)
        //platformSpawner.TriggerWallMode(slowSpeed);
    }

    void Update()
    {
        // 🚧 장애물 생성 테스트를 하는 동안에는 모드가 바뀌면 안 되니까 주석 처리! 🚧
        /*
        timer += Time.deltaTime;
        if (timer >= nextModeTime)
        {
            ChangeMode();
        }
        */
    }

    void ChangeMode()
    {
        timer = 0f;
        nextModeTime = Random.Range(minModeTime, maxModeTime);
        currentMode = Random.Range(0, 3);

        switch (currentMode)
        {
            case 0: // 일반 모드
                platformSpawner.TriggerNormalMode(normalSpeed);
                player.ChangeState(new RunState(player));
                break;
            case 1: // 파괴벽 모드
                StartCoroutine(ShowWarningRoutine());
                platformSpawner.TriggerWallMode(slowSpeed);
                break;
            case 2: // 비행 모드
                platformSpawner.TriggerFlightMode(normalSpeed);
                player.ChangeState(new FlightState(player));
                break;
        }
    }

    public void EndWallMode()
    {
        platformSpawner.TriggerNormalMode(normalSpeed);
        player.ChangeState(new RunState(player));
        timer = 0f;
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