using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [Header("발판 설정")]
    public List<GameObject> platformPrefabs;
    public float platformLength = 10.0f;

    [Header("평소 속도 (초기값만 씁니다)")]
    public float currentSpeed = 7.0f; // MapManager가 실시간으로 조절합니다.

    [Header("간격 및 장애물 설정 (평소 전용)")]
    public float gapDistance = 3.0f;
    public GameObject obstaclePrefab;
    public float obstacleOffsetY = 1.0f;

    [Header("파괴벽 모드 설정")]
    public GameObject wallPrefab;
    public GameObject longPlatformPrefab;
    public float longPlatformLength = 50.0f;

    private bool isWallMode = false;
    private bool hasSpawnedWall = false;

    private GameObject lastSpawnedPlatform;
    private Vector3 initialSpawnPosition;
    private int spawnCount = 0;

    // 풀매니저 변수
    private PoolManager poolManager;

    void Start()
    {
        // 씬에 있는 기존 PoolManager를 찾아서 자동으로 연결합니다.
        poolManager = FindObjectOfType<PoolManager>();

        initialSpawnPosition = transform.position;

        foreach (GameObject platformPrefab in platformPrefabs)
        {
            SpawnPlatform(platformPrefab);
        }
    }

    void Update()
    {
        // 마지막 발판이 기준점(initialSpawnPosition)을 지나가면 새 발판 생성
        if (lastSpawnedPlatform != null && lastSpawnedPlatform.transform.position.x < initialSpawnPosition.x)
        {
            SpawnPlatform(platformPrefabs[0]);
        }
    }

    // MapManager가 부르는 모드 시작 함수
    public void TriggerWallMode(float speed)
    {
        isWallMode = true;
        hasSpawnedWall = false;
        currentSpeed = speed;
    }

    // MapManager가 부르는 모드 종료 함수
    public void TriggerNormalMode(float speed)
    {
        isWallMode = false;
        currentSpeed = speed;
        spawnCount = 0;
    }

    void SpawnPlatform(GameObject platformPrefab)
    {
        Vector3 spawnPos = initialSpawnPosition;

        // 1. 발판 틈새(Pivot) 보정 계산
        if (lastSpawnedPlatform != null)
        {
            float lastHalf = (lastSpawnedPlatform.name.Contains("Long")) ? longPlatformLength / 2f : platformLength / 2f;
            float currentHalf = (platformPrefab.name.Contains("Long")) ? longPlatformLength / 2f : platformLength / 2f;
            float currentGap = isWallMode ? 0f : gapDistance;

            spawnPos = lastSpawnedPlatform.transform.position + new Vector3(lastHalf + currentHalf + currentGap, 0f, 0f);
        }

        GameObject platform = null;

        // 2. 발판 소환 (파괴벽 모드든 평소 모드든, 기본 발판이면 무조건 대기열에서 꺼내옴!)
        if (poolManager != null && platformPrefab == platformPrefabs[0])
        {
            platform = poolManager.GetPoolItem();
            platform.transform.position = spawnPos;
        }
        else
        {
            // 긴 발판 등 특수한 경우에만 새로 생성
            platform = Instantiate(platformPrefab, spawnPos, Quaternion.identity);
        }

        platform.name = platformPrefab.name;
        lastSpawnedPlatform = platform;

        // 3. 모드별 기믹 추가 (파괴벽 or 가시)
        if (isWallMode)
        {
            // 파괴벽 모드: 발판 틈새 없이 깔면서 거대 벽 소환
            if (!hasSpawnedWall)
            {
                Vector3 wallSpawnPos = spawnPos + new Vector3(15f, 0f, 0f);
                Instantiate(wallPrefab, wallSpawnPos, Quaternion.identity);
                hasSpawnedWall = true;
            }
        }
        else
        {
            // 평소 모드: 가시 장애물 무작위 생성
            spawnCount++;
            if (spawnCount > 2 && Random.Range(0f, 1f) < 0.6f)
            {
                Vector3 obstacleSpawnPos = spawnPos + new Vector3(0f, obstacleOffsetY, 0f);
                GameObject obs = Instantiate(obstaclePrefab, obstacleSpawnPos, Quaternion.identity);
                obs.transform.SetParent(platform.transform);
            }
        }
    }
}