//using Firebase;
//using Firebase.Database;
//using Firebase.Extensions;
//using System.Collections.Generic;
//using UnityEngine;

//public class NetworkManager : MonoBehaviour
//{
//    private DatabaseReference dbRef;

//    void Start()
//    {
//        // 1. Firebase ������ üũ �� �ʱ�ȭ
//        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
//            DependencyStatus dependencyStatus = task.Result;

//            if (dependencyStatus == DependencyStatus.Available)
//            {
//                // 2. !!! �߿� !!! ������ URL�� ���⿡ ��������.
//                // ���� DefaultInstance�� �������ٸ� GetInstance("������URL")�� ����մϴ�.
//                string databaseUrl = "https://gameprojectingdgoncnu2026-default-rtdb.firebaseio.com/";

//                FirebaseApp app = FirebaseApp.DefaultInstance;

//                // URL�� ���������� �����Ͽ� �ν��Ͻ� ��������
//                DatabaseReference reference = FirebaseDatabase.GetInstance(app, databaseUrl).RootReference;

//                dbRef = reference;
//                Debug.Log("Firebase Database ���� ����!");
//            }
//            else
//            {
//                Debug.LogError($"Firebase �������� �ذ��� �� �����ϴ�: {dependencyStatus}");
//            }
//        });
//    }

//    // ���� ����
//    public void UploadScore(string playerName, int score)
//    {
//        UserData data = new UserData(playerName, score);

//        string json = JsonUtility.ToJson(data);
//        string userId = SystemInfo.deviceUniqueIdentifier;

//        dbRef.Child("rankings").Child(userId).SetRawJsonValueAsync(json);
//    }

//    // ��ŷ ����Ʈ ����
//    public void FetchLeaderboard(System.Action<List<UserData>> onLoaded)
//    {
//        dbRef.Child("rankings").OrderByChild("score").LimitToLast(10)
//            .GetValueAsync().ContinueWithOnMainThread(task => {
//                if (task.IsCompleted)
//                {
//                    // ������ �Ľ� �� ���� ���� �� �ݹ� ����
//                    // (������ ������ ����Ʈ ��ȯ ���� ���)
//                }
//            });
//    }
//}

using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance;
    private DatabaseReference dbRef;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        // Firebase ������ üũ
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
        string userId = SystemInfo.deviceUniqueIdentifier;
        dbRef.Child("rankings").Child(userId).GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("기존 점수 확인 실패");
                    return;
                }

                DataSnapshot snapshot = task.Result;

                int oldScore = 0;

                if (snapshot.Exists && snapshot.Child("score").Exists)
                {
                    oldScore = int.Parse(snapshot.Child("score").Value.ToString());
                }

                if (score > oldScore)
                {
                    UserData data = new UserData(playerName, score);
                    string json = JsonUtility.ToJson(data);

                    dbRef.Child("rankings").Child(userId).SetRawJsonValueAsync(json);

                    Debug.Log("최고 점수 갱신!");
                }
                else
                {
                    Debug.Log("기존 점수가 더 높아서 저장 안 함");
                }
            });
    }

    // -------------------------------------------------
    // ��ŷ ����Ʈ �޾ƿ��� (�ݹ� ����)
    public void FetchLeaderboard(System.Action<List<UserData>> onLoaded)
    {
        if (dbRef == null) { Debug.LogError("[NetworkManager] DB�� �ʱ�ȭ���� �ʾҽ��ϴ�."); return; }

        dbRef.Child("rankings")
             .OrderByChild("score")
             .LimitToLast(10)
             .GetValueAsync()
             .ContinueWithOnMainThread(task => {
                 if (task.IsFaulted)
                 {
                     Debug.LogError("[NetworkManager] ��ŷ �����͸� �޾ƿ��µ� �����߽��ϴ�.");
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

                 // ���� ���� ������ �������� ����
                 list.Reverse();
                 onLoaded?.Invoke(list);
             });
    }
}
