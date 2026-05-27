using UnityEngine;

public class DestructibleWall : MonoBehaviour
{
    public int hp = 15; // 부수기 위해 필요한 클릭(연타) 횟수

    //  감독 참조용 변수
    private MapManager mapManager;

    void Start()
    {
        //  씬에 있는 MapManager를 자동으로 찾아 연결합니다.
        mapManager = FindObjectOfType<MapManager>();
        if (mapManager == null) Debug.LogError("DestructibleWall: 씬에 MapManager가 없습니다!");
    }

    // PlayerController에서 연타를 감지하면 부를 함수
    public void TakeDamage()
    {
        if (hp <= 0) return; // 이미 파괴 중이면 중복 실행 방지

        hp--; // 체력 1 감소

        // 타격감 효과 (뒤로 살짝 밀리기)
        transform.position += new Vector3(0.2f, 0, 0);

        if (hp <= 0)
        {
            //  (핵심) 자신을 파괴하기 전에 감독에게 보고!
            if (mapManager != null)
            {
                mapManager.EndWallMode();
            }

            // 체력이 0이 되면 파괴!
            Destroy(gameObject);
        }
    }
}