using UnityEngine;

public interface IPlayerState
{
    void Enter();           // 상태가 시작될 때 1번 실행
    void HandleInput();    // 매 프레임 입력 체크
    void UpdateState();    // 매 프레임 로직 처리
    void Exit();    // 상태가 끝날 때 1번 실행
}
