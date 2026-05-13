using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FloorData
{
    public string name;
    public GameObject prefab;
    [Range(1, 100)] public int weight = 10;
}

public class FloorSpawner : MonoBehaviour
{
    // ────────────────────── 설정 영역 ──────────────────────
    [Header("바닥 풀")]
    public List<FloorData> floorPool = new List<FloorData>();

    [Header("스폰 설정")]
    public int initialChunkCount = 6;      // 시작 시 미리 배치할 Chunk 수
    public float scrollSpeed = 5f;         // 전체 스크롤 속도
    public float recycleX = -20f;          // 화면 밖으로 나가면 파괴되는 X 좌표

    // ────────────────────── 내부 변수 ──────────────────────
    private readonly List<FloorChunk> activeChunks = new List<FloorChunk>();
    private int totalWeight = 0;           // 가중치 합계 (한 번만 계산)

    // -----------------------------------------------------------------
    //  Unity 콜백
    // -----------------------------------------------------------------
    void Start()
    {
        if (floorPool.Count == 0)
        {
            Debug.LogError("[FloorSpawner] floorPool is empty!");
            return;
        }

        // 가중치 총합 계산
        foreach (var data in floorPool) totalWeight += data.weight;

        // 초기 Chunk 배치 (가장 왼쪽부터 차례대로)
        float nextX = 0f;
        for (int i = 0; i < initialChunkCount; i++)
        {
            SpawnFloor(nextX);
            // 방금 만든 Chunk 의 가로 길이만큼 다음 위치 이동
            nextX += GetChunkWidth(activeChunks[^1].gameObject);
        }
    }

    void Update()
    {
        // 스크롤 속도 변경이 실시간으로 반영되게
        foreach (var fc in activeChunks)
        {
            if (fc != null) fc.scrollSpeed = scrollSpeed;
        }
    }

    // -----------------------------------------------------------------
    //  Chunk 생성 / 파괴 로직
    // -----------------------------------------------------------------
    /// <summary>
    /// 가장 오른쪽에 존재하는 Chunk 의 X 좌표와 그 너비를 이용해 새 Chunk 를 만든다.
    /// 현재 남아있는 Chunk 가 없으면 (예외 상황) 0 좌표에서 시작한다.
    /// </summary>
    private void SpawnFloorAtRightmost()
    {
        // 현재 가장 오른쪽 Chunk 가 있으면 그 뒤에, 없으면 0 에서 시작
        float baseX = GetRightmostChunkX();
        float width = GetChunkWidthOfLastSpawned(); // 직전에 만든 Chunk 가 있을 경우 그 너비
        float spawnX = (activeChunks.Count == 0) ? 0f : baseX + width;

        SpawnFloor(spawnX);
    }

    /// <summary>
    /// 실제 Chunk 를 생성하고, 콜백을 연결한다.
    /// </summary>
    private void SpawnFloor(float posX)
    {
        GameObject prefab = GetRandomFloorPrefab();
        GameObject go = Instantiate(prefab, transform);
        go.transform.position = new Vector3(posX, 0f, 0f);

        // FloorChunk 컴포넌트가 없으면 자동으로 추가
        FloorChunk fc = go.GetComponent<FloorChunk>();
        if (fc == null) fc = go.AddComponent<FloorChunk>();

        // 기본 파라미터 설정
        fc.scrollSpeed = scrollSpeed;
        fc.recycleX = recycleX;

        // 파괴 시 Spawner 에게 알리는 콜백 연결
        fc.OnDestroyed = () =>
        {
            // 리스트에서 제거 (null 체크는 안전을 위해)
            activeChunks.Remove(fc);

            // 새 Chunk 를 바로 하나 생성
            SpawnFloorAtRightmost();
        };

        // 현재 관리 리스트에 추가
        activeChunks.Add(fc);
    }

    // -----------------------------------------------------------------
    //  유틸리티
    // -----------------------------------------------------------------
    /// <summary>
    /// 현재 남아있는 Chunk 중 X 좌표가 가장 큰 것을 반환한다.
    /// 없으면 0 을 반환한다.
    /// </summary>
    private float GetRightmostChunkX()
    {
        if (activeChunks.Count == 0) return 0f;

        float maxX = float.MinValue;
        foreach (var fc in activeChunks)
        {
            if (fc != null && fc.transform.position.x > maxX)
                maxX = fc.transform.position.x;
        }
        return maxX;
    }

    /// <summary>
    /// 가장 최근에 생성된 Chunk 의 가로 길이를 반환한다.
    /// (activeChunks 가 비어있을 경우 0 반환)
    /// </summary>
    private float GetChunkWidthOfLastSpawned()
    {
        if (activeChunks.Count == 0) return 0f;
        return GetChunkWidth(activeChunks[^1].gameObject);
    }

    /// <summary>
    /// 가중치를 고려해 랜덤하게 프리팹을 선택한다.
    /// </summary>
    private GameObject GetRandomFloorPrefab()
    {
        int roll = Random.Range(0, totalWeight);
        int sum = 0;
        foreach (var data in floorPool)
        {
            sum += data.weight;
            if (roll < sum) return data.prefab;
        }
        // 안전 장치 : 절대 여기서는 도달하지 않음
        return floorPool[0].prefab;
    }

    /// <summary>
    /// 전달받은 GameObject 의 Renderer 로부터 가로 길이(폭)를 반환한다.
    /// Renderer 가 없으면 10 으로 대체한다(임시 기본값).
    /// </summary>
    private float GetChunkWidth(GameObject go)
    {
        Renderer rend = go.GetComponentInChildren<Renderer>();
        return rend != null ? rend.bounds.size.x : 10f;
    }
}
