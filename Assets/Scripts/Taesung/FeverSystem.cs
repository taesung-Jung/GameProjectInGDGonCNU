using UnityEngine;

public class FeverSystem : MonoBehaviour
{
    public float feverGauge = 0f;
    private bool isFeverMode = false;

    public void UpdateFeverGauge(float amount)
    {
        feverGauge += amount;
        if (feverGauge >= 100f && !isFeverMode) EnterFeverMode();
    }

    private void EnterFeverMode()
    {
        isFeverMode = true;
        // 이태이의 PlayerController에 비행 모드 알림
        // 정태성의 ScoreManager에 점수 배율 업 알림
    }
}
