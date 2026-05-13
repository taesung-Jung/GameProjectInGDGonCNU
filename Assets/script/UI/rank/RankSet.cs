using TMPro;
using UnityEngine;

public class RankSet : MonoBehaviour
{
    public int SetNum;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Awake()
    {
        
    }

    void Update()
    {
        UserList user = GameObject.Find("rankingRecords").GetComponent<UserList>();
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Format("{0}",transform.GetSiblingIndex() + 1);
        transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = user.users[SetNum].UserName;
        transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = string.Format("{0}", user.users[SetNum].Time);
        if (transform.GetSiblingIndex() < 1) 
            return;
        if (user.users[SetNum].Time > user.users[transform.parent.GetChild(transform.GetSiblingIndex()-1).GetComponent<RankSet>().SetNum].Time)
        {
            transform.SetSiblingIndex(transform.GetSiblingIndex() -1);
        }
    }
}
