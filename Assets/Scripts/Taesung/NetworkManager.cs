using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    private DatabaseReference dbRef;

    void Start()
    {
        // Firebase 의존성 체크 및 초기화
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            DependencyStatus dependencyStatus = task.Result;

            if (dependencyStatus == DependencyStatus.Available)
            {
                // ScriptableObject 에셋 로드
                // 반드시 Resources/Config 폴더 안에 FirebaseConfig.asset 이 있어야 함
                FirebaseConfig config = Resources.Load<FirebaseConfig>("Config/FirebaseConfig");

                if (config == null)
                {
                    Debug.LogError("[NetworkManager] Resources/Config/FirebaseConfig 에셋을 찾을 수 없습니다! " +
                                   "Resources 폴더에 FirebaseConfig.asset 을 생성해주세요.");
                    return;
                }

                // Firebase 인스턴스 생성 (URL을 직접 전달)
                FirebaseApp app = FirebaseApp.DefaultInstance;
                dbRef = FirebaseDatabase.GetInstance(app, config.databaseUrl).RootReference;

                Debug.Log($"[NetworkManager] Firebase Database 연결 성공! URL: {config.databaseUrl}");
            }
            else
            {
                Debug.LogError($"[NetworkManager] Firebase 의존성을 해결할 수 없습니다: {dependencyStatus}");
            }
        });
    }

    // -------------------------------------------------
    // 점수 전송
    public void UploadScore(string playerName, int score)
    {
        if (dbRef == null) { Debug.LogError("[NetworkManager] DB가 초기화되지 않았습니다."); return; }

        UserData data = new UserData(playerName, score);
        string json = JsonUtility.ToJson(data);
        string userId = SystemInfo.deviceUniqueIdentifier;

        dbRef.Child("rankings").Child(userId).SetRawJsonValueAsync(json)
            .ContinueWithOnMainThread(t => {
                if (t.IsCompleted) Debug.Log("[NetworkManager] 점수 업로드 완료!");
            });
    }

    // -------------------------------------------------
    // 랭킹 리스트 받아오기 (콜백 형태)
    public void FetchLeaderboard(System.Action<List<UserData>> onLoaded)
    {
        if (dbRef == null) { Debug.LogError("[NetworkManager] DB가 초기화되지 않았습니다."); return; }

        dbRef.Child("rankings")
             .OrderByChild("score")
             .LimitToLast(10)
             .GetValueAsync()
             .ContinueWithOnMainThread(task => {
                 if (task.IsFaulted)
                 {
                     Debug.LogError("[NetworkManager] 랭킹 데이터를 받아오는데 실패했습니다.");
                     return;
                 }

                 DataSnapshot snapshot = task.Result;
                 List<UserData> list = new List<UserData>();

                 foreach (var child in snapshot.Children)
                 {
                     string json = child.GetRawJsonValue();
                     UserData user = JsonUtility.FromJson<UserData>(json);
                     list.Add(user);
                 }

                 // 높은 점수 순으로 내림차순 정렬
                 list.Reverse();
                 onLoaded?.Invoke(list);
             });
    }
}