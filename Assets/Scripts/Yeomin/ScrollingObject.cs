using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    private PlatformSpawner spawner;
    private PoolManager poolManager; // 풀매니저 참조 추가
    public float destroyXPosition = -20f;

    void Start()
    {
        spawner = FindObjectOfType<PlatformSpawner>();
        poolManager = FindObjectOfType<PoolManager>(); // 풀매니저 찾기
    }

    void Update()
    {
        if (spawner != null)
        {
            float currentMoveSpeed = spawner.currentSpeed;
            transform.Translate(Vector3.left * currentMoveSpeed * Time.deltaTime);
        }

        // 화면 왼쪽 바깥으로 나갔을 때의 처리
        if (transform.position.x < destroyXPosition)
        {
            // 오브젝트 이름이 "Platform"이고 "Long"이 들어가지 않은 기본 짧은 발판이라면?
            if (poolManager != null && gameObject.name.Contains("Platform") && !gameObject.name.Contains("Long"))
            {
                // [기존 PoolManager 함수 호출] 파괴하지 않고 반납합니다.
                poolManager.ReturnToPool(gameObject);
            }
            else
            {
                // 가시, 파괴벽, 긴 발판 등은 풀링 대상이 아니므로 깔끔하게 삭제합니다.
                Destroy(gameObject);
            }
        }
    }
}