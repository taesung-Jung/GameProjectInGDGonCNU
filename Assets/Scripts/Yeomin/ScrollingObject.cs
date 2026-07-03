using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    private PlatformSpawner spawner;
    private PoolManager poolManager;

    [Header("기본 삭제 X 좌표")]
    public float destroyXPosition = -20f;

    void Start()
    {
        spawner = FindObjectOfType<PlatformSpawner>();
        poolManager = FindObjectOfType<PoolManager>();
    }

    void Update()
    {
        if (spawner != null)
        {
            float currentMoveSpeed = spawner.currentSpeed;
            transform.Translate(Vector3.left * currentMoveSpeed * Time.deltaTime);
        }

        float actualDestroyX = destroyXPosition;
        if (gameObject.name.Contains("Long"))
        {
            actualDestroyX = -45f; // 긴 발판 전용 삭제 좌표
        }

        if (transform.position.x < actualDestroyX)
        {
            if (poolManager != null && gameObject.name.Contains("Platform") && !gameObject.name.Contains("Long"))
            {
                poolManager.ReturnToPool(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}