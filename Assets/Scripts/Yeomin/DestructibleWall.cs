using UnityEngine;

// 이름만 맞춰주는 임시 스크립트입니다.
public class DestructibleWall : MonoBehaviour
{
    // BreakState에서 호출하는 함수 이름만 만들어 둡니다.
    public bool TakeDamage()
    {
        Debug.Log("임시 벽: 데미지 받음!");
        return true; // 일단 클릭 한 번에 무조건 부서지는 것으로 처리
    }
}