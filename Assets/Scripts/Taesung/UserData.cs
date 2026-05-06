using UnityEngine;

public class UserData
{
    public string userName;   // 플레이어 이름 혹은 닉네임
    public int score;      // 랭킹에 사용할 점수

    public UserData(string name, int score)
    {
        this.userName = name;
        this.score = score;
    }
}
