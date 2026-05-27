using UnityEngine;
using TMPro;
using System.Net;
using Unity.VisualScripting;

public class scoreUpload : MonoBehaviour
{
    string playerName;
    int score;
    void Update()
    {
        playerName = GameObject.Find("playerName").GetComponent<TextMeshProUGUI>().text;
        score = (int)GameObject.Find("Timer").GetComponent<Timer>().time;
    }

    public void Upload()
    {
        GameObject.Find("NetworkManager").GetComponent<NetworkManager>().UploadScore(playerName, score);
    }
}
