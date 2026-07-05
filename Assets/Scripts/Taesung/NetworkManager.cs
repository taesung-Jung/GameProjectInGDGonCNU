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
        // Firebase ������ üũ �� �ʱ�ȭ
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            DependencyStatus dependencyStatus = task.Result;

            if (dependencyStatus == DependencyStatus.Available)
            {
                // ScriptableObject ���� �ε�
                // �ݵ�� Resources/Config ���� �ȿ� FirebaseConfig.asset �� �־�� ��
                FirebaseConfig config = Resources.Load<FirebaseConfig>("Config/FirebaseConfig");

                if (config == null)
                {
                    Debug.LogError("[NetworkManager] Resources/Config/FirebaseConfig ������ ã�� �� �����ϴ�! " +
                                   "Resources ������ FirebaseConfig.asset �� �������ּ���.");
                    return;
                }

                // Firebase �ν��Ͻ� ���� (URL�� ���� ����)
                FirebaseApp app = FirebaseApp.DefaultInstance;
                dbRef = FirebaseDatabase.GetInstance(app, config.databaseUrl).RootReference;

                Debug.Log($"[NetworkManager] Firebase Database ���� ����! URL: {config.databaseUrl}");
            }
            else
            {
                Debug.LogError($"[NetworkManager] Firebase �������� �ذ��� �� �����ϴ�: {dependencyStatus}");
            }
        });
    }

    // -------------------------------------------------
    // ���� ����
    public void UploadScore(string playerName, int score)
    {
        if (dbRef == null)
        {
            Debug.LogError("[NetworkManager] DB가 초기화되지 않았습니다.");
            return;
        }

        UserData data = new UserData(playerName, score);
        string json = JsonUtility.ToJson(data);
        string userId = SystemInfo.deviceUniqueIdentifier;

        Debug.Log($"[NetworkManager] UploadScore 호출됨");
        Debug.Log($"[NetworkManager] 저장 경로: rankings/{userId}");
        Debug.Log($"[NetworkManager] 저장 JSON: {json}");

        dbRef.Child("rankings").Child(userId).SetRawJsonValueAsync(json)
            .ContinueWithOnMainThread(t =>
            {
                if (t.IsCompleted)
                    Debug.Log("[NetworkManager] 점수 업로드 완료!");
            });
    }

    // -------------------------------------------------
    // ��ŷ ����Ʈ �޾ƿ��� (�ݹ� ����)
    public void FetchLeaderboard(System.Action<List<UserData>> onLoaded)
    {
        if (dbRef == null)
        {
            return;
        }

        

        dbRef.Child("rankings")
            .OrderByChild("score")
            .LimitToLast(10)
            .GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    
                    return;
                }

                if (task.IsCanceled)
                {
                    
                    return;
                }

                DataSnapshot snapshot = task.Result;

                
                List<UserData> list = new List<UserData>();

                foreach (var child in snapshot.Children)
                {
                    string json = child.GetRawJsonValue();
                    
                    UserData user = JsonUtility.FromJson<UserData>(json);

                    if (user == null)
                    {
                       continue;
                    }

                    
                    list.Add(user);
                }

                list.Reverse();

                Debug.Log($"[NetworkManager] 최종 리스트 개수 : {list.Count}");

                for (int i = 0; i < list.Count; i++)
                {
                    Debug.Log($"[NetworkManager] [{i}] {list[i].userName} / {list[i].score}");
                }

                onLoaded?.Invoke(list);
            });
    }
}