using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    // 1. 필요한 외부 데이터 연결
    public PoolManager poolManager;       // 만들어둔 재활용 센터(PoolManager)
    public GameObject obstaclePrefab;     // 장애물(가시 등) 원본 프리팹

    // 패턴이 더 짧아졌으니, 다음 패턴이 등장하는 시간도 4초에서 2.5초 정도로 줄여줍니다.
    public float spawnInterval = 2.5f; 
    private float timer = 2.5f;

    // X 간격을 10 -> 3으로 확 줄였습니다! 
    private Vector2[] pattern_Basic = { new Vector2(0, 0), new Vector2(3, 0), new Vector2(6, 0) };

    // 구멍 간격도 12 -> 4로 확 줄였습니다!
    private Vector2[] pattern_Hole = { new Vector2(0, 0), new Vector2(4, 0) };

    // 계단 없는 평지
    private Vector2[] pattern_Stairs = { new Vector2(0, 0), new Vector2(3, 0), new Vector2(6, 0) };

    void Update()
    {
        // 매 프레임마다 시간을 누적 (프레임 속도에 구애받지 않음)
        timer += Time.deltaTime;

        // 누적된 시간이 설정한 간격(3초)을 넘어가면 패턴 생성 시작
        if (timer >= spawnInterval)
        {
            SpawnRandomPattern();
            timer = 0f; // 타이머 초기화 (다시 0초부터 측정)
        }
    }

    void SpawnRandomPattern()
    {
        // Random.Range(최소, 최대): 0, 1, 2 중 랜덤한 숫자 뽑기 (최대값은 포함 안 됨)
        int randomIndex = Random.Range(0, 3);
        Vector2[] selectedPattern = pattern_Basic; // 기본값 할당

        // 뽑힌 숫자에 따라 생성할 패턴 결정
        if (randomIndex == 0) selectedPattern = pattern_Basic;
        else if (randomIndex == 1) selectedPattern = pattern_Hole;
        else if (randomIndex == 2) selectedPattern = pattern_Stairs;

        // 패턴이 생성될 화면 오른쪽 끝의 시작 기준 좌표 (x: 12 위치)
        Vector3 startPosition = new Vector3(12f, -2f, 0f);

        // 선택된 패턴 배열의 길이(개수)만큼 반복해서 발판 생성
        for (int i = 0; i < selectedPattern.Length; i++)
        {
            // PoolManager에게 "발판 하나 줘!" 요청해서 받아오기
            GameObject platform = poolManager.GetPoolItem();
            
            // 받아온 발판의 위치를 (시작 위치 + 패턴에 정의된 오프셋)으로 설정
            platform.transform.position = startPosition + (Vector3)selectedPattern[i];
        }

        // 장애물 생성: 30% 확률로 패턴의 시작 지점 약간 위에 가시 장애물 배치
        if (Random.Range(0, 100) < 30)
        {
            SpawnObstacle(startPosition + new Vector3(2f, 1f, 0f));
        }
    }

    void SpawnObstacle(Vector3 spawnPos)
    {
        // 장애물 프리팹이 등록되어 있다면 지정된 위치에 생성(Instantiate)
        if (obstaclePrefab != null)
        {
            Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);
        }
    }
}