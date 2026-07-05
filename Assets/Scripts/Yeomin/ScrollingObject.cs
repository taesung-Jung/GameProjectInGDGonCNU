using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    private PlatformSpawner spawner;
    private PoolManager poolManager;

    public float destroyXPosition = -20f;

    void Start()
    {
        spawner = FindObjectOfType<PlatformSpawner>();
        poolManager = FindObjectOfType<PoolManager>();
    }

    void Update()
    {
        // 맵 매니저(스포너)의 현재 속도에 맞춰 이동
        if (spawner != null)
        {
            float currentMoveSpeed = spawner.currentSpeed;
            transform.Translate(Vector3.left * currentMoveSpeed * Time.deltaTime);
        }

        // 화면 왼쪽 바깥으로 나갔을 때의 청소 및 재활용 처리
        if (transform.position.x < destroyXPosition)
        {
            // 오브젝트 이름이 "Platform"이고 "Long"이 들어가지 않은 기본 짧은 발판이라면 대기열로 반납
            if (poolManager != null && gameObject.name.Contains("Platform") && !gameObject.name.Contains("Long"))
            {
                poolManager.ReturnToPool(gameObject);
            }
            else
            {
                // 가시, 파괴벽, 긴 발판 등은 풀링 대상이 아니므로 삭제(Destroy)
                Destroy(gameObject);
            }
        }
    }
}