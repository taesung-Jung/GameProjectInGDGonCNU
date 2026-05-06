using UnityEngine;
using UnityEngine.UI;

public class TestRunner : MonoBehaviour
{
    public Button testBtn;

    // 클릭 횟수를 저장할 변수
    private int clickCount = 0;

    private void Awake()
    {
        if (testBtn == null)
        {
            Debug.LogError("TestRunner: testBtn이 연결되지 않았습니다!");
            return;
        }

        testBtn.onClick.AddListener(() =>
        {
            // 1. 클릭 횟수 증가
            clickCount++;

            // 2. 이름 생성 (예: DemoUser1, DemoUser2...)
            string playerName = $"DemoUser{clickCount}";
            int randomScore = Random.Range(1000, 9999);

            // 3. 네트워킹 매니저 찾기
            NetworkManager networkManager = FindObjectOfType<NetworkManager>();

            if (networkManager != null)
            {
                // 점수 업로드
                networkManager.UploadScore(playerName, randomScore);
                Debug.Log($"[Test] 업로드 시도: {playerName} - {randomScore}");

                // 랭킹 즉시 조회 (업로드 후 결과 확인용)
                networkManager.FetchLeaderboard(list =>
                {
                    Debug.Log("<color=yellow>==== 현재 실시간 리더보드 ====</color>");
                    foreach (var u in list)
                    {
                        Debug.Log($"[Rank] {u.userName} : {u.score}");
                    }
                });
            }
            else
            {
                Debug.LogError("씬에 NetworkManager가 없습니다!");
            }
        });
    }
}
