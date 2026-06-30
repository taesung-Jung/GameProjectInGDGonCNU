using UnityEngine;

public class DestructibleWall : MonoBehaviour
{
    public int hp = 8; // 부수기 위해 필요한 클릭(연타) 횟수

    //  감독 참조용 변수
    private MapManager mapManager;

    void Start()
    {
        //  씬에 있는 MapManager를 자동으로 찾아 연결합니다.
        mapManager = FindObjectOfType<MapManager>();
        if (mapManager == null) Debug.LogError("DestructibleWall: 씬에 MapManager가 없습니다!");
    }

    // PlayerController에서 연타를 감지하면 부를 함수
    public bool TakeDamage()
    {
        if (hp <= 0) return false;

        hp--;
        transform.position += new Vector3(0.2f, 0, 0); // 타격감

        if (hp <= 0)
        {
            // 1. 플레이어를 찾아 강제로 오른쪽으로 이동 (벽 파괴 효과)
            PlayerController player = FindObjectOfType<PlayerController>();
            if (player != null)
            {
                // 벽을 부수고 나서 오른쪽으로 1.5만큼 강제 이동
                player.transform.position += new Vector3(1.5f, 0, 0);

                // 또는 물리적인 힘을 원한다면: player.rb.AddForce(Vector2.right * 5f, ForceMode2D.Impulse);
            }

            if (mapManager != null)
            {
                mapManager.EndWallMode();
            }

            Destroy(gameObject);
            return true;
        }
        return false;
    }
}