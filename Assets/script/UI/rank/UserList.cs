using UnityEngine;
using System.Collections.Generic;
using TMPro; // 꼭 필요!


public class UserList : MonoBehaviour
{
    public List<UserInfo> users = new List<UserInfo>();
    public GameObject recordPrefab;

    void Update()
    {
        Ranking_Arrange();
    }
    void Awake()
    {
        
        add_user("hello", 10f);
        add_user("hello2", 20f);
        add_user("hello3", 5f);
        add_user("hello7", 1f);
        Debug.Log(users[2].UserName);
        
    }
    void Ranking_Arrange()
    {
        Transform content = transform.GetChild(0).GetChild(0).transform;
        for (int i = 0; i < content.childCount; i++)
        {
            content.GetChild(i).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -20 + -110*i);
        }
    }
    public void add_user(string username, float time)
    {
        for (int i = 0; i < users.Count; i++)
        {
            if (users[i].UserName == username)
            {
                users[i].Time = time;
                return;
            }
        }
        users.Add(new UserInfo(username, time));
        GameObject prefab = Instantiate(recordPrefab, transform.GetChild(0).GetChild(0).transform);
        prefab.GetComponent<RankSet>().SetNum = users.Count -1;
    }
}

public class UserInfo
{
    public string UserName;
    public float Time;

    public UserInfo(string username, float time)
    {
        UserName = username;
        Time = time;
    }
}