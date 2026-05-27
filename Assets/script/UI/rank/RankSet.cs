using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class RankSet : MonoBehaviour
{
    public int SetNum;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        GameObject.Find("NetworkManager").GetComponent<NetworkManager>().FetchLeaderboard(UpdateInfo);
    }

    void UpdateInfo(List<UserData> list)
    {
        UserList user = GameObject.Find("rankingRecords").GetComponent<UserList>();
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Format("{0}",transform.GetSiblingIndex() + 1);
        transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = list[SetNum].userName;
        transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = string.Format("{0}", list[SetNum].score);
        if (transform.GetSiblingIndex() < 1) 
            return;
        if (list[SetNum].score > list[transform.parent.GetChild(transform.GetSiblingIndex()-1).GetComponent<RankSet>().SetNum].score)
        {
            transform.SetSiblingIndex(transform.GetSiblingIndex() -1);
        }
    }
}
