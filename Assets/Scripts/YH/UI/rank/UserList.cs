using UnityEngine;
using System.Collections.Generic;


public class UserList : MonoBehaviour
{
    public GameObject recordPrefab;
        
    void Update()
    {
        Ranking_Arrange();
    }
    void OnEnable()
    {
        GameObject.Find("NetworkManager").GetComponent<NetworkManager>().FetchLeaderboard(add_user);
    }
    void Ranking_Arrange()
    {
        Transform content = transform.GetChild(0).GetChild(0).transform;
        
        for (int i = 0; i < content.childCount; i++)
        {
            if (i > 3)
            {
                content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 470 + 110 * i);
            }
            content.GetChild(i).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -20 + -110*i);
        }
    }
    public void add_user(List<UserData> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            GameObject prefab = Instantiate(recordPrefab, transform.GetChild(0).GetChild(0).transform);
            prefab.GetComponent<RankSet>().SetNum = i;
        }
        
    }
}