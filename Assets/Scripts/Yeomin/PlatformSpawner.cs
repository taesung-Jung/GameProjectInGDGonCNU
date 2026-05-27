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
    public GameObject longPlatformPrefab; //  방금 만든 긴 발판 프리팹칸!
    public float longPlatformLength = 50.0f; //  긴 발판의 길이 (아까 Width에 적은 숫자)

    private bool isWallMode = false;
    private bool hasSpawnedWall = false;

    private GameObject lastSpawnedPlatform;
    private Vector3 initialSpawnPosition;
    private int spawnCount = 0;
    private PoolManager poolManager;

    void Start()
    {
        // 씬에 있는 기존 PoolManager를 찾아서 연결합니다.
        poolManager = FindObjectOfType<PoolManager>();

        initialSpawnPosition = transform.position;
        foreach (GameObject platformPrefab in platformPrefabs)
        {
            SpawnPlatform(platformPrefab);
        }
    }

    void Update()
    {
        if (lastSpawnedPlatform != null && lastSpawnedPlatform.transform.position.x < initialSpawnPosition.x)
        {
            SpawnPlatform(platformPrefabs[0]);
        }
    }

    //  MapManager가 부르는 모드 시작 함수 (느린 속도 인자로 받음)
    public void TriggerWallMode(float speed)
    {
        isWallMode = true;
        hasSpawnedWall = false;
        currentSpeed = speed; // 전체 속도를 느리게 설정
    }

    //  MapManager가 부르는 모드 종료 함수 (평소 속도 인자로 받음)
    public void TriggerNormalMode(float speed)
    {
        isWallMode = false;
        currentSpeed = speed; // 전체 속도를 빠르게 복구
        spawnCount = 0; // 카운트 초기화 (가시 소환 패턴 리셋)
    }

    void SpawnPlatform(GameObject platformPrefab)
    {
        Vector3 spawnPos = initialSpawnPosition;

        if (lastSpawnedPlatform != null)
        {
            float lastHalf = (lastSpawnedPlatform.name.Contains("Long")) ? longPlatformLength / 2f : platformLength / 2f;
            float currentHalf = (platformPrefab.name.Contains("Long")) ? longPlatformLength / 2f : platformLength / 2f;
            float currentGap = isWallMode ? 0f : gapDistance;

            spawnPos = lastSpawnedPlatform.transform.position + new Vector3(lastHalf + currentHalf + currentGap, 0f, 0f);
        }

        GameObject platform = null; // 기본 변수 선언

        if (isWallMode)
        {
            // 파괴벽 모드: 긴 발판은 기존처럼 Instantiate로 생성
            platform = Instantiate(platformPrefab, spawnPos, Quaternion.identity);
            platform.name = platformPrefab.name;
            lastSpawnedPlatform = platform;

            if (!hasSpawnedWall)
            {
                Vector3 wallSpawnPos = spawnPos + new Vector3(15f, 0f, 0f);
                Instantiate(wallPrefab, wallSpawnPos, Quaternion.identity);
                hasSpawnedWall = true;
            }
        }
        else
        {
            // 평소 모드: 기본 짧은 발판(platformPrefabs[0])일 때만 풀매니저에서 빌려옵니다!
            if (poolManager != null && platformPrefab == platformPrefabs[0])
            {
                //  [기존 PoolManager 함수 호출]
                platform = poolManager.GetPoolItem();
                platform.transform.position = spawnPos; // 위치 세팅
            }
            else
            {
                // 그 외의 경우가 있다면 예외적으로 생성
                platform = Instantiate(platformPrefab, spawnPos, Quaternion.identity);
            }

            platform.name = platformPrefab.name;
            lastSpawnedPlatform = platform;
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