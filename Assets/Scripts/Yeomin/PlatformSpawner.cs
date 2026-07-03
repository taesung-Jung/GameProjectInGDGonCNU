using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [Header("발판 설정")]
    public List<GameObject> platformPrefabs;
    public float platformLength = 10.0f;

    [Header("평소 속도")]
    public float currentSpeed = 7.0f;

    [Header("간격 및 장애물 설정")]
    public float gapDistance = 3.0f;
    public GameObject obstaclePrefab;
    public float obstacleOffsetY = 1.0f;

    [Header("파괴벽 모드 설정")]
    public GameObject wallPrefab;
    public GameObject longPlatformPrefab;
    public float longPlatformLength = 50.0f;

    [Header("비행 모드 설정")]
    public GameObject flightObstaclePrefab;
    public float flightSpawnInterval = 1.5f;
    public float minFlightY = -3.0f;
    public float maxFlightY = 3.0f;
    public Transform spawnPoint;

    private bool isWallMode = false;
    private bool hasSpawnedWall = false;
    private bool isFlightMode = false;

    private GameObject lastSpawnedPlatform;
    private Vector3 initialSpawnPosition;
    private int spawnCount = 0;

    private PoolManager poolManager;

    void Start()
    {
        poolManager = FindObjectOfType<PoolManager>();
        initialSpawnPosition = transform.position;

        if (longPlatformPrefab != null)
        {
            SpawnPlatform(longPlatformPrefab);
        }
        else if (platformPrefabs.Count > 0)
        {
            SpawnPlatform(platformPrefabs[0]);
        }
    }

    float GetHalfLength(GameObject platform)
    {
        if (platform == null) return platformLength / 2f;

        if (platform.name.Contains("Long"))
        {
            return longPlatformLength / 2f;
        }
        return platformLength / 2f;
    }

    void Update()
    {
        GameObject sceneCanvas = GameObject.Find("SceneCanvas");
        if (sceneCanvas != null)
        {
            Scenemanager sm = sceneCanvas.GetComponent<Scenemanager>();
            if (sm != null && sm.ready) return;
        }

        if (!isFlightMode && lastSpawnedPlatform != null)
        {
            float lastHalf = GetHalfLength(lastSpawnedPlatform);
            float rightEdgeX = lastSpawnedPlatform.transform.position.x + lastHalf;

            if (rightEdgeX < initialSpawnPosition.x + 20.0f)
            {
                if (platformPrefabs.Count > 0)
                {
                    SpawnPlatform(platformPrefabs[0]);
                }
            }
        }
    }

    public void TriggerWallMode(float speed)
    {
        isWallMode = true;
        isFlightMode = false;
        hasSpawnedWall = false;
        currentSpeed = speed;
    }

    public void TriggerFlightMode(float speed)
    {
        Debug.Log(" 비행 모드 발동! 바닥 생성을 멈추고 기둥을 소환합니다.");
        isFlightMode = true;
        isWallMode = false;
        currentSpeed = speed;

        StartCoroutine(SpawnFlightObstacleRoutine());
    }

    public void TriggerNormalMode(float speed)
    {
        if (isFlightMode)
        {
            lastSpawnedPlatform = Instantiate(platformPrefabs[0], initialSpawnPosition + new Vector3(platformLength, 0, 0), Quaternion.identity);
        }

        isWallMode = false;
        isFlightMode = false;
        currentSpeed = speed;
        spawnCount = 0;

        StopAllCoroutines();
    }

    IEnumerator SpawnFlightObstacleRoutine()
    {
        if (flightObstaclePrefab == null || spawnPoint == null)
        {
            Debug.LogError(" 비행 장애물 프리팹이나 SpawnPoint가 연결되지 않았습니다! 인스펙터를 확인하세요.");
            yield break;
        }

        while (isFlightMode)
        {
            float randomY = Random.Range(minFlightY, maxFlightY);
            Vector3 spawnPosition = new Vector3(spawnPoint.position.x, randomY, 0);

            GameObject obstacle = Instantiate(flightObstaclePrefab, spawnPosition, Quaternion.identity);
            obstacle.name = flightObstaclePrefab.name;

            yield return new WaitForSeconds(flightSpawnInterval);
        }
    }

    void SpawnPlatform(GameObject platformPrefab)
    {
        Vector3 spawnPos = initialSpawnPosition;

        if (lastSpawnedPlatform != null)
        {
            float lastHalf = GetHalfLength(lastSpawnedPlatform);
            float currentHalf = GetHalfLength(platformPrefab);
            float currentGap = isWallMode ? 0f : gapDistance;

            spawnPos = lastSpawnedPlatform.transform.position + new Vector3(lastHalf + currentHalf + currentGap, 0f, 0f);
        }

        GameObject platform = null;

        if (poolManager != null && platformPrefab == platformPrefabs[0])
        {
            platform = poolManager.GetPoolItem();
            platform.transform.position = spawnPos;
        }
        else
        {
            platform = Instantiate(platformPrefab, spawnPos, Quaternion.identity);
        }

        platform.name = platformPrefab.name;
        lastSpawnedPlatform = platform;

        if (isWallMode)
        {
            if (!hasSpawnedWall)
            {
                Vector3 wallSpawnPos = spawnPos + new Vector3(15f, 0f, 0f);
                Instantiate(wallPrefab, wallSpawnPos, Quaternion.identity);
                hasSpawnedWall = true;
            }
        }
        else
        {
            if (!isFlightMode)
            {
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
}