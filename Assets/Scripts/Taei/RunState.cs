using UnityEngine;

public class RunState : IPlayerState
{
    private PlayerController player;

    public RunState(PlayerController playerController)
    {
        this.player = playerController;
    }

    public void Enter()
    {
        // 착지 시 점프 횟수를 0으로 초기화하여 다시 2단 점프가 가능하게 합니다.
        player.jumpCount = 0;

        player.anim.SetBool("Grounded", true);
    }

    public void HandleInput()
    {
        // 마우스 왼쪽 버튼 클릭 시 점프 상태로 변경
        if (Input.GetMouseButtonDown(0))
        {
            player.ChangeState(new JumpState(player));
        }

        // 마우스 오른쪽 버튼 클릭 시 슬라이드 상태로 변경
        if (Input.GetMouseButtonDown(1))
        {
            player.ChangeState(new SlideState(player));
        }
    }

    public void UpdateState() { }
    public void Exit() { }
}