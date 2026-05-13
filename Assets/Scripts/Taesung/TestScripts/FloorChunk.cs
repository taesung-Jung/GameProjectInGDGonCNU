using UnityEngine;

public class FloorChunk : MonoBehaviour
{
    [HideInInspector] public float scrollSpeed;
    public float recycleX = -30f; // 화면 왼쪽으로 벗어나는 지점

    // 바닥이 사라질 때 Spawner에게 알리기 위한 델리게이트/이벤트
    public System.Action OnDestroyed;

    void Update()
    {
        // 왼쪽으로 이동
        transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime, Space.World);

        // 일정 지점을 넘어가면 자기 자신을 파괴
        if (transform.position.x < recycleX)
        {
            OnDestroyed?.Invoke(); // Spawner에게 알림
            Destroy(gameObject);   // 객체 제거
        }
    }
}
